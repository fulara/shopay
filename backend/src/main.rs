#[macro_use] extern crate juniper;
extern crate juniper_iron;
extern crate iron;
extern crate mount;
extern crate uuid;

use std::sync::{
    RwLock,
    Arc,
};

use std::collections::HashMap;

use uuid::Uuid;

use mount::Mount;
use iron::prelude::*;
use juniper_iron::GraphQLHandler;
use juniper::FieldResult;
use juniper_iron::GraphiQLHandler;
use std::collections::HashSet;

#[derive(GraphQLObject, Clone, Debug)]
#[graphql(description="just an item")]
struct Item {
    id : String,
    name: String,
    description: String,
    category: String,
    amount: String,
    unit: String,

    bought : bool,
    last_update_timestamp : i32,
}

#[derive(GraphQLInputObject)]
struct ItemState {
    id : String,
    name: String,
    description: String,
    category: String,
    amount: String,
    unit: String,

    bought : bool,
    last_update_timestamp : i32, //secs since epoch  - easier to serialize as millis :)
}

#[derive(GraphQLInputObject)]
struct ClientUpdate {
    items : Vec<ItemState>,

    last_server_update : i32,
}

#[derive(GraphQLObject)]
struct ClientUpdateResponse {
    present : Vec<Item>,
    deleted : Vec<String>,

    server_time : i32,
}

#[derive(GraphQLObject)]
struct ClearResult {
    count : i32,
}

struct MemDb {
    items : HashMap<String, Item>,

    historical_items : HashMap<String, Item>,
}

impl MemDb {
    fn new() -> Self {
        Self {
            items : HashMap::new(),
            historical_items: HashMap::new(),

        }
    }

    fn items(&self) -> Vec<Item> {
        self.items.clone().into_iter().map(|(_k,v)| v ).into_iter().collect()
    }

    fn clear(&mut self) -> i32 {
        let len = self.items.len();

        self.items.clear();

        len as i32  // graphql doesnt seem to handle usize.
    }

    fn update(&mut self, update : ClientUpdate) -> Vec<String> {
        let client_ids : HashSet<_> = update.items.iter().map(|i| i.id.clone()).collect();
        let mut items_removed_by_others = Vec::new();
        update.items.into_iter().for_each(|new| {
            if let Some(local) =  self.items.get_mut(&new.id) {
                if local.last_update_timestamp < new.last_update_timestamp {
                    local.last_update_timestamp = new.last_update_timestamp;
                    local.bought = new.bought;
                    local.category = new.category;
                    local.name = new.name;
                    local.description = new.description;
                    local.amount = new.amount;
                    local.unit = new.unit;
                }
            } else if self.historical_items.contains_key(&new.id) {
                items_removed_by_others.push(new.id.clone());
            } else {
                self.items.insert(new.id.clone(), Item {
                    id : new.id.clone(),
                    name : new.name,
                    category : new.category,
                    description: new.description,
                    amount: new.amount,
                    unit: new.unit,

                    bought : new.bought,

                    last_update_timestamp : new.last_update_timestamp,
                });
            }
        });

        //now check any item locally known if their ts is > last client server update then its a new item to be pushed.
        Self::detect_and_remove_items_deleted_by_client(client_ids, update.last_server_update, &mut self.items, &mut self.historical_items);

        items_removed_by_others
    }

    fn detect_and_remove_items_deleted_by_client(client_ids : HashSet<String>, client_last_server_update : i32, local_items : &mut HashMap<String, Item>, historical_items : &mut HashMap<String, Item>) {
        local_items.retain(| local_id, local_item | {
            if !client_ids.contains(local_id) {
                if local_item.last_update_timestamp < client_last_server_update {
                    historical_items.insert(local_id.clone(), local_item.clone());
                    false
                } else {
                    true
                }
            }  else {
                true
            }
        })
    }

    //placeholder
    fn generate_id() -> String {
        Uuid::new_v4().to_string()
    }
}

struct Context {
    db : Arc<RwLock<MemDb>>,
}

impl juniper::Context for Context {
}

struct Query;

graphql_object!(Query: Context |&self| {
    field apiVersion() -> &str {
        "1.0"
    }

//    {items{name,category,bought}}
    field items(&executor) -> FieldResult<Vec<Item>> {
        let context = executor.context();
        Ok(context.db.read().unwrap().items())
    }

    // to query an union ( not used atm ): field custom -> Enum { Concrete(..), NotConcrete(..) }
    // { custom { ... on Concrete { x } ... on NotConcrete { y } }}
});

struct Mutation;

graphql_object!(Mutation: Context |&self| {
    //mutation{ clear {count}}
    field clear(&executor) -> FieldResult<ClearResult> {
        Ok(ClearResult { count : executor.context().db.write().unwrap().clear()})
    }

//    mutation {
//        update(data :{ lastServerUpdate : 1, items:  [ { id:"4", name:"potato", description:"supa", category:"cat", amount: "1.0", unit: "m",  bought:false, updatedAt:2  } ]   }) {
//            present {
//              id,
//              name,
//              category,
//              bought,
//              lastUpdate,
//            }
//           serverTime
//          }
//        }
    field update(&executor, data : ClientUpdate) -> FieldResult<ClientUpdateResponse> {
        let lsu = data.last_server_update;

        let context = executor.context();
        let db = &mut context.db.write().unwrap();
        let deleted = db.update(data);

        Ok(ClientUpdateResponse {
            deleted,
            present: db.items(),
            server_time : lsu + 10, // to make life easier for now.
        })
    }
});

//type Schema = juniper::RootNode<'static, Query, Mutation>;

fn main() {

    let mut mount = Mount::new();

    let x = Arc::new(RwLock::new(MemDb::new()));
    let ct = move |_ : &mut Request| {
        println!("ctx is being created.");
        Ok(Context { db : x.clone()})
    };

    let graphql_endpoint = GraphQLHandler::new(ct,
                                               Query {},
                                               Mutation {});

    let graphiql_endpoint = GraphiQLHandler::new("/graphql");

    mount.mount("/", graphiql_endpoint);
    mount.mount("/graphql", graphql_endpoint);

    let chain = Chain::new(mount);
    //chain.link_before(logger_before);
    //chain.link_after(logger_after);

    let host = "0.0.0.0:4000".to_owned();
    println!("GraphQL server started on {}", host);
    Iron::new(chain).http(host.as_str()).unwrap();

}
