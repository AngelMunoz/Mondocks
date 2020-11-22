// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Mondocks.Queries
open MongoDB.Driver
open MongoDB.Bson

let createUsers minAge maxAge = 
    let random  = Random()
    insert {
        use_collection "users"
        with_documents 
            [
                {| name = "Peter"; age = random.Next(minAge, maxAge); |}
                {| name = "Sandra"; age = random.Next(minAge, maxAge); |}
                {| name = "Mike"; age = random.Next(minAge, maxAge); |}
                {| name = "Perla"; age = random.Next(minAge, maxAge); |}
                {| name = "Updateme"; age = 1; |}
                {| name = "Deleteme"; age = 50; |}
            ]
    }

let updateUser (name: string) (newName: string) =
    update {
        use_collection "users"
        with_updates
            [
                { q = {| name = name |} 
                  u = {| name = newName; age = 5 |}
                  multi = Some false
                  upsert = Some false
                  collation = None 
                  arrayFilters = None
                  hint = None }
            ]
    }


let deleteUser (name: string) = 
    delete {
        use_collection "users"
        with_deletes [
            { q = {| name = name |}
              limit = 1
              collation = None
              hint = None
              comment = None }
        ]
    }

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
    let client = MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("mondocks")

    let userscmd = createUsers 15 50
    let result = db.RunCommand<BsonDocument>(JsonCommand userscmd)
    printfn $"InsertResult: [{result.ToJson()}]"

    let over20 = getUsersOverAge 20
    let result = db.RunCommand<BsonDocument>(JsonCommand over20)
    printfn $"FindResult: [{result.ToJson()}]"

    let updatecmd = updateUser "Updateme" "Updated"
    let result = db.RunCommand<BsonDocument>(JsonCommand updatecmd)
    printfn $"UpdateResult: [{result.ToJson()}]"

    let deletecmd = deleteUser "Deleteme"
    let result = db.RunCommand<BsonDocument>(JsonCommand deletecmd)
    printfn $"DeleteResult: [{result.ToJson()}]"
    
    0 // return an integer exit code