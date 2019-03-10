#[macro_use] extern crate juniper;
extern crate juniper_iron;
extern crate iron;
extern crate mount;
extern crate uuid;

use std::sync::{
    RwLock,
    Arc,
};

use uuid::Uuid;

use mount::Mount;
use iron::prelude::*;
use juniper_iron::GraphQLHandler;
use juniper::FieldResult;
use juniper_iron::GraphiQLHandler;

#[derive(GraphQLObject)]
#[graphql(description="Potatko")]
struct Potato {
    name: String,
    category: String,
}

#[derive(GraphQLObject, Clone, Debug)]
#[graphql(description="just an item")]
struct Item {
    id : String,
    name: String,
    category: String,
    bought : bool,
}

#[derive(GraphQLInputObject)]
struct AddedItem {
    name: String,
    category: String,
    bought : bool,
}

#[derive(GraphQLInputObject)]
struct RemovedItem {
    id : String,
    name: String,
}

#[derive(GraphQLInputObject)]
struct ChangedItem {
    id: String,
}

#[derive(GraphQLInputObject)]
struct ClientUpdate {
    added : Vec<AddedItem>,
    removed : Vec<RemovedItem>,
    changed : Vec<ChangedItem>,
}

#[derive(GraphQLInputObject)]
#[graphql(description="Placeholder")]
struct AddItem {
    name: String,
}

#[derive(GraphQLObject)]
#[graphql(description="Placeholder")]
struct InputPlaceholderRet {
    name: String,
}

#[derive(GraphQLObject)]
struct ClearResult {
    count : i32,
}

struct MemDb {
    items : Vec<Item>,
}

impl MemDb {
    fn new() -> Self {
        Self {
            items : Vec::new(),
        }
    }

    fn items(&self) -> Vec<Item> {
        self.items.clone()
    }

    fn add(&mut self, new : AddItem) {
        self.items.push(Item {
            id : Self::generate_id(),
            name : new.name,
            bought : false,
            category : "a".to_string(),
        })
    }

    fn clear(&mut self) -> i32 {
        let len = self.items.len();

        self.items.clear();

        len as i32  // :(
    }

    fn update(&mut self, update : ClientUpdate) {
        update.added.into_iter().for_each(|new| {
            self.items.push(Item {
                id : Self::generate_id(),
                name : new.name,
                category : new.category,
                bought : new.bought,
            })
        });

        update.removed.into_iter().for_each(|removed| self.items.retain(|item| item.id != removed.id));

        update.changed.into_iter().for_each(|updated| self.items.iter().filter(|item| item.id == updated.id).for_each(|item| ()));
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
// mutation{ addItem(newItem : { name:"potatko" }) { name }}
    field addItem(&executor, new_item: AddItem) -> FieldResult<InputPlaceholderRet> {
        executor.context().db.write().unwrap().add(new_item);
        Ok(InputPlaceholderRet { name : "a".to_string() })
    }

//    mutation{ clear {count}}
    field clear(&executor) -> FieldResult<ClearResult> {
        Ok(ClearResult { count : executor.context().db.write().unwrap().clear()})
    }

    //mutation{ update(data: { added :[ { name:"p", category:"c", bought:false }], removed:[], changed:[], }) { name}}
    field update(&executor, data : ClientUpdate) -> FieldResult<Vec<Item>> {
        let context = executor.context();
        let  db = &mut context.db.write().unwrap();
        db.update(data);
        Ok(db.items())
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
