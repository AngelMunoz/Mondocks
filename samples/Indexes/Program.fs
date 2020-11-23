open System
open MongoDB.Driver
open MongoDB.Bson
open Mondocks.Database
open Mondocks.Types


let createIndexCmd =
    let myindex = 
        index "user_email_unique_idx" {
            key (dict(["email", box 1.0]))
            unique true
        }
    let myindex2 = 
        index "lastName_idx" {
            key (dict(["lastName", box -1.0]))
        }
    let myindex3 = 
        index "lastName2_idx" {
            key (dict(["lastName2", box -1.0]))
        }
    let myindex4 = 
        index "lastName3_idx" {
            key (dict(["lastName3", box -1.0]))
        }
        
    createIndexes "users" {
        indexes [myindex; myindex2; myindex3; myindex4]
    }

let dropIndexCmd = 
    dropIndexes "users" {
        index "lastName_idx"
    }

let dropIndexesCmd = 
    dropIndexes "users" {
        index [
            "lastName2_idx"
            "lastName3_idx"
        ]
    }


[<EntryPoint>]
let main argv =
    let client = MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("mondocks")

    printfn $"{createIndexCmd}\n{dropIndexCmd}"

    let createIndexResult = db.RunCommand<CreateIndexResult>(JsonCommand createIndexCmd)
    let dropIndexResult = db.RunCommand<DropIndexResult>(JsonCommand dropIndexCmd)
    let dropIndexesResult = db.RunCommand<DropIndexResult>(JsonCommand dropIndexesCmd)
    
    printfn $"%A{createIndexResult.ToJson()}\n{dropIndexResult.ToJson()}\n{dropIndexesResult.ToJson()}"
    0 // return an integer exit code// return an integer exit code