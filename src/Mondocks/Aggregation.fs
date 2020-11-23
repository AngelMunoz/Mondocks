namespace Mondocks

open System
open System.Collections.Generic
open Mondocks.Types

module Aggregation =

    [<AutoOpen>]
    module Count =
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

            [<CustomOperation("collection")>]
            member __.Count(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, count: string) =
                { state with count = count }

            [<CustomOperation("query")>]
            member __.Query(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, query: 'TDocument) =
                { state with query = Some query }

            [<CustomOperation("limit")>]
            member __.Limit(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, limit: int) =
                { state with limit = Some limit }

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

        let count = CountCommandBuilder()


    [<AutoOpen>]
    module Distinct =
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

            [<CustomOperation("key")>]
            member __.Key(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, key: string) =
                { state with key = key }

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

        let distinct (collection: string) = DistinctCommandBuilder(collection)
