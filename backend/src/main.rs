#[macro_use] extern crate juniper;
extern crate juniper_iron;
extern crate iron;
extern crate mount;

use std::sync::{
    RwLock,
    Arc,
};

use mount::Mount;
use iron::prelude::*;
use juniper::EmptyMutation;
use juniper_iron::GraphQLHandler;
use juniper::FieldResult;
use juniper_iron::GraphiQLHandler;

#[derive(GraphQLObject)]
#[graphql(description="Potatko")]
struct Potato {
    name: String,
    category: String,
}

#[derive(GraphQLInputObject)]
#[graphql(description="Item jest super kewl")]
struct Item {
    name: String,
    category: String,
}

struct Context {
    i : Arc<RwLock<i32>>,
}

impl juniper::Context for Context {
}

struct Query;

graphql_object!(Query: Context |&self| {
    field apiVersion() -> &str {
        "1.0"
    }

    field potato(&executor, item : Item) -> FieldResult<Potato> {
        let context = executor.context();
        Ok(Potato { name : item.name, category: "cat".to_owned()})
    }
});

struct Mutation;

graphql_object!(Mutation: Context |&self| {

    field createHuman(&executor, new_item: Item) -> FieldResult<Potato> {
        let context : &Context = executor.context();
        let mut data : &mut i32  = &mut context.i.write().unwrap();

        *data += 1;
        Ok(Potato { name : data.to_string(), category: new_item.category })
    }
});

type Schema = juniper::RootNode<'static, Query, Mutation>;

fn main() {
    let mut mount = Mount::new();

    let x = Arc::new(RwLock::new(0));
    let ct = move |_ : &mut Request| {
        println!("ctx is being created.");
        Ok(Context { i : x.clone()})
    };

    let graphql_endpoint = GraphQLHandler::new(ct,
                                               Query {},
                                               Mutation {});

    let graphiql_endpoint = GraphiQLHandler::new("/graphql");

    mount.mount("/", graphiql_endpoint);
    mount.mount("/graphql", graphql_endpoint);

    let mut chain = Chain::new(mount);
    //chain.link_before(logger_before);
    //chain.link_after(logger_after);

    let host = "0.0.0.0:4000".to_owned();
    println!("GraphQL server started on {}", host);
    Iron::new(chain).http(host.as_str()).unwrap();
}
