open System
open MongoDB.Driver
open MongoDB.Bson
open Mondocks.Queries
open Mondocks.Aggregation
open Mondocks.Types

let createUsers minAge maxAge = 
    let random  = Random()
    insert "users" {
        documents
            [
                // an anonymous object that does not include a null _id
                {| name = "Peter"; age = random.Next(minAge, maxAge); |}
                {| name = "Sandra"; age = random.Next(minAge, maxAge); |}
                {| name = "Mike"; age = random.Next(minAge, maxAge); |}
                {| name = "Perla"; age = random.Next(minAge, maxAge); |}
                {| name = "Perla"; age = random.Next(minAge, maxAge); |}
                {| name = "Updateme"; age = 1; |}
                {| name = "Deleteme"; age = 50; |}
            ]
    }

let countCmd = 
    count {
        collection "users"
        query {| age = 50 |}
    }

let distinctCmd = 
    distinct "users" {
        key "name"
    }



[<EntryPoint>]
let main argv =
    let client = MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("mondocks")

    let userscmd = createUsers 15 50
    let result = db.RunCommand<InsertResult>(JsonCommand userscmd)
    printfn $"InsertResult: %A{result}"

    let countResult = db.RunCommand<CountResult>(JsonCommand countCmd)
    let distinctResult = db.RunCommand<DistinctResult<string>>(JsonCommand distinctCmd)

    printfn $"%A{countResult.ToJson()}\n%A{distinctResult.ToJson()}"

    0 // return an integer exit code// return an integer exit code