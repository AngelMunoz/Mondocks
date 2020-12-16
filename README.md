[mongodb extended json]: https://docs.mongodb.com/manual/reference/mongodb-extended-json/

# Mondocks

![nuget](https://badgen.net/nuget/v/mondocks)
[![Binder](https://notebooks.gesis.org/binder/badge_logo.svg)](https://notebooks.gesis.org/binder/v2/gh/AngelMunoz/Mondocks/HEAD?filepath=CRUD-Guide.ipynb)

> ```
> dotnet add package Mondocks --version 0.2.0
> ```

> This library is based on the mongodb extended json spec and mongodb manual reference
>
> https://docs.mongodb.com/manual/reference/mongodb-extended-json/ > https://docs.mongodb.com/manual/reference/command/

This library provides a set of familiar tools if you work with mongo databases and can be a step into more F# goodies, it doesn't prevent you from using the usual MongoDB/.NET driver so you can use them side by side. It also can help you if you have a lot of flexible data inside your database as oposed to the usual strict schemas that F#/C# are used to from SQL tools, this provides a DSL that allow you to create `MongoDB Commands (raw queries)` leveraging the dynamism of anonymous records since they behave almost like javascript objects.
Writing commands should be almost painless these commands produce a JSON string that can be utilized directly on your application or even copy/pasted into the mongo shell.
Commands are kind of a version of `raw sql queries` but they allow you to do what you already know how to do without much changes to the objects you might be manipulating already.
Ideally this library is meant to be used mostly with records and anonymous records to imitate `mongodb` queries

## Sample Usage

Check out this quick sample of what you can do right now

> You can also check this [gist](https://gist.github.com/AngelMunoz/35cf2bc439da9969664f9987f7109ee3)

```fsharp
open System
open MongoDB.Driver
open MongoDB.Bson
open Mondocks.Queries
open Mondocks.Types

// a named record
type User = { _id: ObjectId; name: string; age: int }

let createUsers minAge maxAge =
    let random  = Random()
    insert "users" {
        documents
            [   // an anonymous object that does not include a null _id
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
            [ { // you can do mongo queries like
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
        deletes
           [ { q = {| name = name |}
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
    printfn $"FindResult Ok: %f{result.ok}"
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

> Thanks for the early feedback in twitter from Isaac, Zaid, Alexey, Alexander, and the F# community
> you can follow it on the first [issue](https://github.com/AngelMunoz/Mondocks/issues/1)

### Goals

- Emit 100% compatible json with https://docs.mongodb.com/manual/reference/command/ and https://docs.mongodb.com/manual/reference/mongodb-extended-json
- Provide Tools that are familiar from those who come from other environments to be productive since day 1
- Provide Types to ease the transition between command execution and it's return types
- Provide CE's to generate sub types of the main command definitions (e.g. index)

### Non Eequired Extras

- provide helpers to write different syntax (e.g. `filter (fun m -> m.prop = value)`, `filter ("prop" gt 10)`)

### Non Goals

- Convert this into a document mapper
- Provide 100% of the mongo commands
- Provide a 100% F# idiomatic solution

This is a work in progress, you can help providing feedback about it's usage
