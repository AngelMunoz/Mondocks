#r "nuget: FsMake, 0.6.1"

open FsMake
open System
open System.IO

let libraries = [ "Mondocks"; "Mondocks.Fable"; "Mondocks.Net" ]

let NugetApiKey = EnvVar.getOrFail "NUGET_DEPLOY_KEY"

[<Literal>]
let PackageVersion = "0.5.0"

let fsSources =
    Glob.create "*.fsx"
    |> Glob.toPaths
    |> Seq.append (
        Glob.createWithRootDir "src" "**/*.fs"
        |> Glob.add "**/*.fsi"
        |> Glob.exclude "**/fable_modules/*.fs"
        |> Glob.exclude "**/fable_modules/**/*.fs"
        |> Glob.exclude "**/obj/*.fs"
        |> Glob.exclude "**/obj/**/*.fs"
        |> Glob.toPaths
    )

let outDir = Path.GetFullPath("./dist")

module Operations =
    type FantomasError =
        | PendingFormat
        | FailedToFormat

    let cleanDist =
        make {
            let! ctx = Make.context

            try
                Directory.Delete(outDir, true)
            with ex ->
                ctx.Console.WriteLine(Console.warn $"We tried to delete '{outDir}' but '{ex.Message}' happened")
        }

    let fantomas (command: string) =
        make {
            let! result =
                Cmd.createWithArgs "dotnet" [ "fantomas"; command; yield! fsSources ]
                |> Cmd.checkExitCode Cmd.ExitCodeCheckOption.CheckCodeNone
                |> Cmd.redirectOutput Cmd.RedirectToBoth
                |> Cmd.result

            match result.ExitCode with
            | 0 -> return ()
            | 99 -> return! Make.fail (nameof FantomasError.PendingFormat)
            | _ -> return! Make.fail (nameof FantomasError.FailedToFormat)
        }

    let dotnet (args: string) =
        Cmd.createWithArgs "dotnet" (args.Split(' ') |> List.ofArray) |> Cmd.run

    let nugetPush (nupkg: string, apiKey) =
        Cmd.createWithArgs
            "dotnet"
            [ "nuget"
              "push"
              nupkg
              "--skip-duplicate"
              "-s"
              "https://api.nuget.org/v3/index.json"
              "-k" ]
        |> Cmd.argSecret apiKey
        |> Cmd.run

module Steps =
    let installTools =
        Step.create "Install Tools" { do! Operations.dotnet "tool restore" }

    let restore = Step.create "Restore" { do! Operations.dotnet "restore" }

    let clean = Step.create "Clean" { do! Operations.cleanDist }

    let packNugets =
        Step.create "Pack Nugets" {
            let! ctx = Step.context
            Console.info "Generating NuGet Package" |> ctx.Console.WriteLine

            for packable in libraries do
                do! Operations.dotnet $"pack src/{packable}/{packable}.fsproj -p:Version={PackageVersion} -o {outDir}"
        }


    let build =
        Step.create "build" { do! Operations.dotnet "build Mondocks.sln --no-restore" }

    let format = Step.create "format" { do! Operations.fantomas "format" }


    let pushNugets =
        Step.create "nuget" {
            let! apiKey = NugetApiKey

            for library in libraries do
                let nupkName = $"./dist/{library}.{PackageVersion}.nupkg"
                do! Operations.nugetPush (nupkName, apiKey)
        }

module Pipelines =

    let cleanDist = Pipeline.create "clean:dist" { run Steps.clean }

    let restore =
        Pipeline.create "restore" {
            run Steps.installTools
            run Steps.restore
        }

    let format = Pipeline.createFrom restore "format" { run Steps.format }

    let packNuget =
        Pipeline.createFrom restore "build:nuget" {
            run Steps.clean
            run Steps.packNugets
        }

    let pushNugets = Pipeline.createFrom packNuget "push:nuget" { run Steps.pushNugets }


    let build = Pipeline.createFrom restore "build" { run Steps.build }

Pipelines.create {
    add Pipelines.cleanDist
    add Pipelines.restore
    add Pipelines.format
    add Pipelines.packNuget
    add Pipelines.pushNugets
    add Pipelines.build
    default_pipeline Pipelines.build
}
|> Pipelines.runWithArgsAndExit fsi.CommandLineArgs
