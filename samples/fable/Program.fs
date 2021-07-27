module Expressive.Program

open System

open Glutinum.Express
open Glutinum.ExpressServeStaticCore

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop

open Mondocks.Types
open Mondocks.Queries
open Mondocks.Fable.Json

type Post =
    { id: string
      title: string
      postedAt: DateTime }


[<ImportMemberAttribute("./sample.js")>]
let savePost (cmd: obj) : Promise<InsertResult> = jsNative

[<ImportMemberAttribute("./sample.js")>]
let findPosts (cmd: obj) : Promise<Post list> = jsNative


let addPostCmd (post: Post) =
    insert "posts" { documents [ post ] } |> ToCommand

let findPostsCmd =
    find "posts" { sort {| title = -1 |} }
    |> ToCommand

let bodyParser =
    importDefault<Glutinum.BodyParser.BodyParser.IExports> "body-parser"

let app = express.express ()

app.``use`` (bodyParser.json ())

app.get (
    "/posts",
    fun (req: Request) (res: Response) ->
        promise {
            let! result = findPosts findPostsCmd
            console.log (result)
            res.json result
        }
        |> ignore
)

app.post (
    "/posts",
    fun (req: Request) (res: Response) ->
        match req.body with
        | Some body ->
            promise {
                let post = (body :?> Post)

                let! result =
                    { post with
                          id = Guid.NewGuid().ToString() }
                    |> addPostCmd
                    |> savePost

                res.sendStatus(201).json result
            }
            |> ignore
        | None ->
            promise {
                res
                    .sendStatus(400)
                    .send {| message = "No post data" |}
            }
            |> ignore
)




[<EntryPoint>]
let main argv =
    app.listen (3000, "0.0.0.0", (fun _ -> printfn "Started app at 0.0.0.0:3000"))
    |> ignore

    console.log (addPostCmd, findPostsCmd)

    0 // return an integer exit code
