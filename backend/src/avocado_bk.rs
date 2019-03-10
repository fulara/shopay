//jus tbecause i am lazy left it here for later. :) ignore this file as its unused!

#[macro_use]
extern crate avocado_derive;
extern crate avocado;

#[macro_use]
extern crate serde_derive;
extern crate serde;
#[macro_use]
extern crate bson;
#[macro_use]
extern crate magnet_derive;
extern crate magnet_schema;

use avocado::prelude::*;

//#[derive(Debug, Clone, Serialize, Deserialize, BsonSchema, Doc)]
//struct User {
//    #[serde(rename = "_id")]
//    id: Uid<User>,
//    legal_name: String,
//}


fn main() -> AvocadoResult<()> {
//    let client = Client::with_uri("mongodb://localhost:27017/")?;
//
//    let db = client.db("test_db");
//    let users_novalidate: Collection<User> = db.empty_collection_novalidate()?;
//
//    println!("insert result is: {:?}", users_novalidate.insert_one(&User { id : Uid::new_oid()?, legal_name : "potato".to_owned()} ));
//
//    Ok(())
}