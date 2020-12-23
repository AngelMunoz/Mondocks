namespace Mondocks.Types

open MongoDB.Bson.Serialization.Attributes
open Mondocks

type Collation =
    { locale: string
      caseLevel: Option<bool>
      caseFirst: Option<bool>
      strength: Option<bool>
      numericOrdering: Option<bool>
      alternate: Option<bool>
      maxVariable: Option<bool>
      backwards: Option<bool> }

    interface IBuilder with
        member __.ToJSON() = Json.Serialize __

type DeleteQuery<'Delete, 'Hint, 'Comment> =
    { /// that represents the matching criteria
      q: 'Delete
      /// amount of documents to delete. use `0` to delete all matching criteria
      limit: int
      collation: Option<Collation>
      hint: Option<'Hint>
      comment: Option<'Comment> }

    interface IBuilder with
        member __.ToJSON() = Json.Serialize __

type UpdateQuery<'Query, 'Update, 'Hint> =
    { /// the matching criteria
      q: 'Query
      /// A document or update pipeline
      u: 'Update
      upsert: Option<bool>
      multi: Option<bool>
      collation: Option<Collation>
      /// An array of filter documents that determines which array elements to modify for an update operation on an array field.
      arrayFilters: Option<seq<obj>>
      hint: 'Hint }

[<BsonIgnoreExtraElementsAttribute>]
type Cursor<'T> = { firstBatch: seq<'T>; ns: string }

type FindResult<'T> = { cursor: Cursor<'T>; ok: float }

[<BsonIgnoreExtraElements>]
type InsertResult = { n: int; ok: float }

type UpdateResult = { n: int; nModified: int; ok: float }
type DeleteResult = { n: int; ok: float }

[<BsonIgnoreExtraElements>]
type CreateIndexResult =
    { createdCollectionAutomatically: bool
      numIndexesBefore: int
      numIndexesAfter: int
      ok: float }

[<BsonIgnoreExtraElements>]
type DropIndexResult = { nIndexesWas: int; ok: float }

type CountResult = { n: int; ok: float }

type DistinctResult<'TValue> = { values: seq<'TValue>; ok: float }

module Builders =

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
            { state with
                  caseLevel = Some caseLevel }

        [<CustomOperation("case_first")>]
        member __.CaseFirst(state: Collation, caseFirst: bool) =
            { state with
                  caseFirst = Some caseFirst }

        [<CustomOperation("strength")>]
        member __.Strength(state: Collation, strength: bool) = { state with strength = Some strength }

        [<CustomOperation("numeric_ordering")>]
        member __.NumericOrdering(state: Collation, numericOrdering: bool) =
            { state with
                  numericOrdering = Some numericOrdering }

        [<CustomOperation("alternate")>]
        member __.Alternate(state: Collation, alternate: bool) =
            { state with
                  alternate = Some alternate }

        [<CustomOperation("max_variable")>]
        member __.MaxVariable(state: Collation, maxVariable: bool) =
            { state with
                  maxVariable = Some maxVariable }

        [<CustomOperation("backwards")>]
        member __.Backwards(state: Collation, backwards: bool) =
            { state with
                  backwards = Some backwards }



    type DeleteQueryBuilder() =
        member __.Yield _ =
            { q = {|  |}
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
            { state with
                  collation = Some collation }

        [<CustomOperation("hint")>]
        member __.Hint(state: DeleteQuery<'Delete, 'Hint, 'Comment>, hint: 'Hint) = { state with hint = Some hint }

        [<CustomOperation("comment")>]
        member __.Comment(state: DeleteQuery<'Delete, 'Hint, 'Comment>, comment: 'Comment) =
            { state with comment = Some comment }

    type UpdateQueryBuilder() =
        member __.Yield _ =
            { q = {|  |}
              u = {|  |}
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
            { state with
                  collation = Some collation }


        [<CustomOperation("array_filters")>]
        member __.ArrayFilters(state: UpdateQuery<'Query, 'Update, 'Hint>, arrayFilters: obj seq) =
            { state with
                  arrayFilters = Some arrayFilters }

        member this.ArrayFilters(state: UpdateQuery<'Query, 'Update, 'Hint>, arrayFilters: 'T seq) =
            this.ArrayFilters(state, arrayFilters |> Seq.map box)

        [<CustomOperation("hint")>]
        member __.Hint(state: UpdateQuery<'Query, 'Update, 'Hint>, hint: 'Hint) = { state with hint = hint }


    let collation (locale: string) = CollationBuilder(locale)
    let deletequery = DeleteQueryBuilder()
    let updatequery = UpdateQueryBuilder()
