namespace Mondocks.Database

open Mondocks.Fable.Json

[<AutoOpen>]
module Database =
    let index (name: string) = IndexBuilder(name, Serialize)

    let createIndexes (collection: string) =
        CreateIndexesBuilder(collection, Serialize)

    let dropIndexes (collection: string) =
        DropIndexesBuilder(collection, Serialize)
