namespace Mondocks.Database

open Mondocks.Net.Json

[<AutoOpen>]
module Database =
    let index (name: string) =
        IndexBuilder(name, Serializer.Serialize)

    let createIndexes (collection: string) =
        CreateIndexesBuilder(collection, Serializer.Serialize)

    let dropIndexes (collection: string) =
        DropIndexesBuilder(collection, Serializer.Serialize)
