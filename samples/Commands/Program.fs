// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Mondocks.Queries
open MongoDB.Driver
open MongoDB.Bson
open Mondocks.Types

let createUsers minAge maxAge = 
    let random  = Random()
    insert "users" {
        documents 
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
    update "users" {
        updates
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
    delete "users" {
        deletes [
            { q = {| name = name |}
              limit = 1
              collation = None
              hint = None
              comment = None }
        ]
    }

// Define a function to construct a message to print
let getUsersOverAge (age: int) =
    find "users" {
        filter {| age = {| ``$gt``= age |} |}
        limit 2
        skip 1
    }
type User = { _id: ObjectId; name: string; age: int }
[<EntryPoint>]
let main argv =
    let client = MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("mondocks")

    let userscmd = createUsers 15 50
    let result = db.RunCommand<InsertResult>(JsonCommand userscmd)
    printfn $"InsertResult: %A{result}"

    let over20 = getUsersOverAge 20
    let result = db.RunCommand<FindResult<User>>(JsonCommand over20)
    printfn $"FindResult Ok: %d{result.ok}"
    result.cursor.firstBatch |> Seq.iter (fun value -> printfn $"%A{value}")

    let updatecmd = updateUser "Updateme" "Updated"
    let result = db.RunCommand<UpdateResult>(JsonCommand updatecmd)
    printfn $"UpdateResult: %A{result}"

    let deletecmd = deleteUser "Deleteme"
    let result = db.RunCommand<DeleteResult>(JsonCommand deletecmd)
    printfn $"DeleteResult: %A{result}"
    
    0 // return an integer exit code