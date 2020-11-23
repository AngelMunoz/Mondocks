# Mondocks
![nuget](https://badgen.net/nuget/v/mondocks/pre)
> # WIP
> Get the early bits
>
> ```
> dotnet add package Mondocks --version 0.1.0-beta3
> ```


> This library is based on the mongodb manual reference
>
> https://docs.mongodb.com/manual/reference/command/


The mongodb .NET driver is made entirely for C# and it expects you to pass data and information in a way C# can (reflection, inheritance among others). This ain't a problem but sometimes it makes working with complex object shapes quite clunky making you define types for almost every shape of your data, that can be fixed partialy with anonymous objects but it's clunky still.


I tried to go in a different way providing a kind of DSL that allow you to create `MongoDB Commands (raw queries)` leveraging the dynamism of anonymous records since they behave almost like javascript objects.

Writing commands should be almost painless these commands produce a JSON string that can be utilized directly on your application or even copy/pasted into the mongo shell

Commands are kind of a version of `raw sql queries` but they allow you to do what you already know how to do without much changes to the objects you might be manipulating already.
Ideally this library is meant to be used mostly with records and anonymous records to imitate `mongodb` queries

## Sample Usage

Check out this quick sample of what you can do right now

```fsharp
open System
open MongoDB.Driver
open MongoDB.Bson
open Mondocks.Queries
open Mondocks.Types

// a Full object here
type User = { _id: ObjectId; name: string; age: int }

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
                {| name = "Updateme"; age = 1; |}
                {| name = "Deleteme"; age = 50; |}
            ]
    }

let updateUser (name: string) (newName: string) =
    update "users" {
        updates
            [
                { // you can do mongo queries like 
                  // {| ``$or`` = [] |} -> { $or: [] }
                  q = {| name = name |}
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
        // equivalent to a mongo query filter 
        // { age: { $gt: 10} }
        filter {| age = {| ``$gt``= age |} |}
        limit 2
        skip 1
    }

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
    
    0 // return an integer exit code// return an integer exit code
```

If you want to see what else is available check the `samples` directory


Thanks for the early feedback in twitter from Isaac, Zaid, Alexey, Alexander, and the F# community
you can follow it on the first [issue](https://github.com/AngelMunoz/Mondocks/issues/1)


### Goals
- Emit 100% compatible json with https://docs.mongodb.com/manual/reference/command/
- Provide Types to ease the transition between command execution and it's return
- Provide CE's to generate sub types of the main command definitions (e.g. index)

### Non Eequired Extras
- provide helpers to write different syntax (e.g. `filter (fun m -> m.prop = value)`, `filter ("prop" gt 10)`)

### Non Goals
- convert this into a document mapper
- provide 100% of the mongo commands

This is a work in progress, you can help providing feedback about it's usage

![Samples](./2020-11-22_14-51.png)
