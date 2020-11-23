open System
open MongoDB.Driver
open MongoDB.Bson
open Mondocks.Database
open Mondocks.Types


let indexCmd =
    let myindex = 
        index "user_email_unique_idx" {
            key (dict(["email", box 1.0]))
            unique true
        }
    let myindex2 = 
        index "lastName_idx" {
            key (dict(["lastName", box -1.0]))
        }
        
    createIndexes "users" {
        indexes [myindex; myindex2]
        comment {| message ="Some Comment" |}
    }



[<EntryPoint>]
let main argv =
    let client = MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("mondocks")

    printfn $"{indexCmd}"
    
    let indexResult = db.RunCommand<BsonDocument>(JsonCommand indexCmd)
    printfn $"{indexResult.ToJson()}"
    0 // return an integer exit code// return an integer exit code