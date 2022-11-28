namespace Mondocks.Types

open System

type SerializerFn = obj -> string

type CountResult = { n: int; ok: float }
type DistinctResult<'TValue> = { values: seq<'TValue>; ok: float }

type CreateIndexResult =
    { createdCollectionAutomatically: bool
      numIndexesBefore: int
      numIndexesAfter: int
      ok: float }

type DropIndexResult = { nIndexesWas: int; ok: float }
type InsertResult = { n: int; ok: float }

type FindResult<'T> =
    { cursor: {| firstBatch: seq<'T>; ns: string |}
      ok: float }

type UpdateResult = { n: int; nModified: int; ok: float }
type DeleteResult = { n: int; ok: float }

type Collation =
    { locale: string
      caseLevel: Option<bool>
      caseFirst: Option<bool>
      strength: Option<bool>
      numericOrdering: Option<bool>
      alternate: Option<bool>
      maxVariable: Option<bool>
      backwards: Option<bool> }


type DeleteQuery<'Delete, 'Hint, 'Comment> =
    {
        /// that represents the matching criteria
        q: 'Delete
        /// amount of documents to delete. use `0` to delete all matching criteria
        limit: int
        collation: Option<Collation>
        hint: Option<'Hint>
        comment: Option<'Comment>
    }


type UpdateQuery<'Query, 'Update, 'Hint> =
    {
        /// the matching criteria
        q: 'Query
        /// A document or update pipeline
        u: 'Update
        upsert: Option<bool>
        multi: Option<bool>
        collation: Option<Collation>
        /// An array of filter documents that determines which array elements to modify for an update operation on an array field.
        arrayFilters: Option<seq<obj>>
        hint: 'Hint
    }

[<Obsolete("This module is deprecated and will be removed on a future release, please use `Mondocks.Types.Extras`")>]
module Builders =
    [<Obsolete("This construct has moved to the `Mondocks.Types.Extras` namespace")>]
    type CollationBuilder(locale: string) =

        member __.Yield _ =
            { locale = ""
              caseLevel = None
              caseFirst = None
              strength = None
              numericOrdering = None
              alternate = None
              maxVariable = None
              backwards = None }

        member __.Run(state: Collation) = { state with locale = locale }

        [<CustomOperation("case_level")>]
        member __.CaseLevel(state: Collation, caseLevel: bool) =
            { state with caseLevel = Some caseLevel }

        [<CustomOperation("case_first")>]
        member __.CaseFirst(state: Collation, caseFirst: bool) =
            { state with caseFirst = Some caseFirst }

        [<CustomOperation("strength")>]
        member __.Strength(state: Collation, strength: bool) = { state with strength = Some strength }

        [<CustomOperation("numeric_ordering")>]
        member __.NumericOrdering(state: Collation, numericOrdering: bool) =
            { state with numericOrdering = Some numericOrdering }

        [<CustomOperation("alternate")>]
        member __.Alternate(state: Collation, alternate: bool) =
            { state with alternate = Some alternate }

        [<CustomOperation("max_variable")>]
        member __.MaxVariable(state: Collation, maxVariable: bool) =
            { state with maxVariable = Some maxVariable }

        [<CustomOperation("backwards")>]
        member __.Backwards(state: Collation, backwards: bool) =
            { state with backwards = Some backwards }

    [<Obsolete("This construct has moved to the `Mondocks.Types.Extras` namespace")>]
    type DeleteQueryBuilder() =
        member __.Yield _ =
            { q = Unchecked.defaultof<_> ()
              limit = 0
              collation = None
              hint = None
              comment = None }

        member __.Run(state: DeleteQuery<'Delete, 'Hint, 'Comment>) = state


        [<CustomOperation("query")>]
        member __.Query(state: DeleteQuery<'Delete, 'Hint, 'Comment>, query: 'Delete) = { state with q = query }

        [<CustomOperation("limit")>]
        member __.Limit(state: DeleteQuery<'Delete, 'Hint, 'Comment>, limit: int) = { state with limit = limit }

        [<CustomOperation("collation")>]
        member __.Collation(state: DeleteQuery<'Delete, 'Hint, 'Comment>, collation: Collation) =
            { state with collation = Some collation }

        [<CustomOperation("hint")>]
        member __.Hint(state: DeleteQuery<'Delete, 'Hint, 'Comment>, hint: 'Hint) = { state with hint = Some hint }

        [<CustomOperation("comment")>]
        member __.Comment(state: DeleteQuery<'Delete, 'Hint, 'Comment>, comment: 'Comment) =
            { state with comment = Some comment }

    [<Obsolete("This construct has moved to the `Mondocks.Types.Extras` namespace")>]
    type UpdateQueryBuilder() =
        member __.Yield _ =
            { q = Unchecked.defaultof<_> ()
              u = Unchecked.defaultof<_> ()
              upsert = None
              multi = None
              collation = None
              arrayFilters = None
              hint = None }

        member __.Run(state: UpdateQuery<'Query, 'Update, 'Hint>) = state


        [<CustomOperation("query")>]
        member __.Query(state: UpdateQuery<'Query, 'Update, 'Hint>, query: 'Query) = { state with q = query }

        [<CustomOperation("update")>]
        member __.Update(state: UpdateQuery<'Query, 'Update, 'Hint>, update: 'Update) = { state with u = update }

        [<CustomOperation("upsert")>]
        member __.Upsert(state: UpdateQuery<'Query, 'Update, 'Hint>, upsert: bool) = { state with upsert = Some upsert }

        [<CustomOperation("multi")>]
        member __.Multi(state: UpdateQuery<'Query, 'Update, 'Hint>, multi: bool) = { state with multi = Some multi }

        [<CustomOperation("collation")>]
        member __.Collation(state: UpdateQuery<'Query, 'Update, 'Hint>, collation: Collation) =
            { state with collation = Some collation }


        [<CustomOperation("array_filters")>]
        member __.ArrayFilters(state: UpdateQuery<'Query, 'Update, 'Hint>, arrayFilters: obj seq) =
            { state with arrayFilters = Some arrayFilters }

        member this.ArrayFilters(state: UpdateQuery<'Query, 'Update, 'Hint>, arrayFilters: 'T seq) =
            this.ArrayFilters(state, arrayFilters |> Seq.map box)

        [<CustomOperation("hint")>]
        member __.Hint(state: UpdateQuery<'Query, 'Update, 'Hint>, hint: 'Hint) = { state with hint = hint }
