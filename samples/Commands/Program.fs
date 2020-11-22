// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Mondocks.Query.Find
open MongoDB.Driver
open MongoDB.Bson

// Define a function to construct a message to print
let getUsersOverAge (age: int) =
    find {
        use_collection "users"
        with_filter {| age = {| ``$gt``= age |} |}
        with_limit 2
        with_skip 1
    }

[<EntryPoint>]
let main argv =
    let over20 = getUsersOverAge 20
    printfn $"Command: [{over20}]"
    let client = MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("mondocks")
    let result = db.RunCommand<BsonDocument>(JsonCommand over20)
    printfn $"Result: [{result.ToJson()}]"
    
    0 // return an integer exit code