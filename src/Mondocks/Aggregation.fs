namespace Mondocks

open System
open System.Collections.Generic
open Mondocks.Types

module Aggregation =

    [<AutoOpen>]
    module Count =

        /// <summary>
        /// Represents a `Count` command
        /// for more information take a look at <a href="https://docs.mongodb.com/manual/reference/command/count/#syntax">count syntax</a>
        /// </summary>
        type CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment> =
            { count: string
              query: Option<'TDocument>
              limit: Option<int>
              skip: Option<int>
              hint: Option<'Hint>
              readConcern: Option<'ReadConcern>
              collation: Option<Collation>
              comment: Option<'Comment> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __


        type CountCommandBuilder() =

            member __.Yield _ =
                { count = ""
                  query = None
                  limit = None
                  skip = None
                  hint = None
                  readConcern = None
                  collation = None
                  comment = None }

            member __.Run(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>) = (state :> IBuilder).ToJSON()

            ///<summary>The name of the collection  to query against</summary>
            /// <example>
            ///    count {
            ///        collection "users"
            ///        query {| age = 50 |}
            ///    }
            /// </example>
            [<CustomOperation("collection")>]
            member __.Count(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, count: string) =
                { state with count = count }

            /// <summary>
            ///  Optional object that is going to be used to filter the find query
            /// </summary>
            /// <example>
            ///     count {
            ///       collection "users"
            ///       // counts all the users with the name "Frank"
            ///       query {| name = "Frank"|}
            ///     }
            /// </example>
            [<CustomOperation("query")>]
            member __.Query(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, query: 'TDocument) =
                { state with query = Some query }

            /// <summary>
            /// Optional. An integer value used to omit the first n amount of documents from the find query
            /// Use together with Limit to paginate results
            /// </summary>
            /// <example>
            ///     count {
            ///       collection "users"
            ///       // ... omit other fields
            ///       // omits the first 10 results of the count query
            ///       skip 10
            ///       // ... omit other fields
            ///     }
            /// </example>
            [<CustomOperation("limit")>]
            member __.Limit(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, limit: int) =
                { state with limit = Some limit }

            /// <summary>
            /// Optional. An integer value used to take at most n amount of documents
            /// Use together with Skip to paginate results
            /// </summary>
            /// <example>
            ///     count {
            ///       collection "users"
            ///       // ... omit other fields
            ///       // takes only 10 documents from the count query
            ///       limit 10
            ///       // ... omit other fields
            ///     }
            /// </example>
            [<CustomOperation("skip")>]
            member __.Skip(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, skip: int) =
                { state with skip = Some skip }

            [<CustomOperation("hint")>]
            member __.Hint(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, hint: 'Hint) =
                { state with hint = Some hint }

            [<CustomOperation("collation")>]
            member __.Collation(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, collation: Collation) =
                { state with
                      collation = Some collation }

            [<CustomOperation("comment")>]
            member __.Comment(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, comment: 'Comment) =
                { state with comment = Some comment }

        /// <summary>Creates an count command for documents in the specified collection</summary>
        /// <param name="collection">The name of the collection to perform this query against</param>
        /// <returns>returns a <see cref="Mondocks.Aggregation.Count.CountCommandBuilder">CountCommandBuilder</see></returns>
        let count = CountCommandBuilder()


    [<AutoOpen>]
    module Distinct =

        /// <summary>
        /// Represents a `Distinct` command
        /// for more information take a look at <a href="https://docs.mongodb.com/manual/reference/command/distinct/#syntax">count syntax</a>
        /// </summary>
        type DistinctCommand<'TDocument, 'ReadConcern, 'Comment> =
            { distinct: string
              key: string
              query: Option<'TDocument>
              readConcern: Option<'ReadConcern>
              collation: Option<Collation>
              comment: Option<'Comment> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type DistinctCommandBuilder(collection: string) =

            member __.Yield _ =
                { distinct = ""
                  key = ""
                  query = None
                  readConcern = None
                  collation = None
                  comment = None }

            member __.Run(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>) =
                ({ state with distinct = collection } :> IBuilder)
                    .ToJSON()

            /// <summary>
            /// A key used to retreive values from the query result documents
            /// </summary>
            /// <example>
            ///     distinct "users" {
            ///       key "email"
            ///     }
            /// </example>
            [<CustomOperation("key")>]
            member __.Key(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, key: string) =
                { state with key = key }

            /// <summary>
            /// Optional. A filter to query against the collection
            /// </summary>
            /// <example>
            ///     distinct "users" {
            ///       // gets all of the distinc emails from users named "Frank"
            ///       key "email"
            ///       query {| name = "Frank"|}
            ///     }
            /// </example>
            [<CustomOperation("query")>]
            member __.Query(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, query: 'TDocument) =
                { state with query = Some query }

            [<CustomOperation("read_concern")>]
            member __.ReadConcern(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, readConcern: 'ReadConcern) =
                { state with
                      readConcern = Some readConcern }

            [<CustomOperation("collation")>]
            member __.Collation(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, collation: Collation) =
                { state with
                      collation = Some collation }

            [<CustomOperation("collation")>]
            member __.Comment(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, comment: 'Comment) =
                { state with comment = Some comment }

        /// <summary>Creates an update command for documents in the specified collection</summary>
        /// <param name="collection">The name of the collection to perform this query against</param>
        /// <returns>returns a <see cref="Mondocks.Aggregation.Distinct.DistinctCommandBuilder">DistinctCommandBuilder</see></returns>
        let distinct (collection: string) = DistinctCommandBuilder(collection)
