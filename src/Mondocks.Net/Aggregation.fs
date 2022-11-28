namespace Mondocks.Aggregation

open Mondocks.Net.Json

[<AutoOpen>]
module Aggregation =
    /// <summary>Creates an count command for documents in the specified collection</summary>
    /// <returns>returns a <see cref="Mondocks.Aggregation.Count.CountCommandBuilder">CountCommandBuilder</see></returns>
    let count = CountCommandBuilder(Serializer.Serialize)

    /// <summary>Creates an update command for documents in the specified collection</summary>
    /// <param name="collection">The name of the collection to perform this query against</param>
    /// <returns>returns a <see cref="Mondocks.Aggregation.Distinct.DistinctCommandBuilder">DistinctCommandBuilder</see></returns>
    let distinct (collection: string) =
        DistinctCommandBuilder(collection, Serializer.Serialize)
