namespace Mondocks.Queries

open Mondocks.Net.Json

[<AutoOpen>]
module Query =
    /// <summary>creates a find and modify command for a single document</summary>
    /// <param name="collection">The name of the collection to perform this query against</param>
    /// <returns>returns a <see cref="Mondocks.Queries.Find.FindAndModifyBuilder">FindAndModifyBuilder</see></returns>
    let findAndModify (collection: string) =
        FindAndModifyBuilder(collection, Serializer.Serialize)

    /// <summary>Creates a find command for documents in the specified collection</summary>
    /// <param name="collection">The name of the collection to perform this query against</param>
    /// <returns>returns a <see cref="Mondocks.Queries.Find.FindBuilder">FindBuilder</see></returns>
    let find (collection: string) =
        FindBuilder(collection, Serializer.Serialize)

    /// <summary>Creates a delete command for documents in the specified collection</summary>
    /// <param name="collection">The name of the collection to perform this query against</param>
    /// <returns>returns a <see cref="Mondocks.Queries.Delete.DeleteBuilder">DeleteBuilder</see></returns>
    let delete (collection: string) =
        DeleteBuilder(collection, Serializer.Serialize)

    /// <summary>Creates an insert command for documents in the specified collection</summary>
    /// <param name="collection">The name of the collection to perform this query against</param>
    /// <returns>returns a <see cref="Mondocks.Queries.Insert.InsertCommandBuilder">InserCommandBuilder</see></returns>
    let insert (collection: string) =
        InsertCommandBuilder(collection, Serializer.Serialize)

    /// <summary>Creates an update command for documents in the specified collection</summary>
    /// <param name="collection">The name of the collection to perform this query against</param>
    /// <returns>returns a <see cref="Mondocks.Queries.Update.UpdateCommandBuilder">UpdateCommandBuilder</see></returns>
    let update (collection: string) =
        UpdateCommandBuilder(collection, Serializer.Serialize)
