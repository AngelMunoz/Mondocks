/// Provides some types and builders that might be useful
/// but are not required for the core functionality.
/// Often, these types don't require to be serialized since you need the object
/// when doing a command
namespace Mondocks.Types.Extras

open Mondocks.Types

type CollationBuilder(locale: string) =

    member val Locale = locale

    member inline __.Yield _ =
        { locale = ""
          caseLevel = None
          caseFirst = None
          strength = None
          numericOrdering = None
          alternate = None
          maxVariable = None
          backwards = None }

    member inline this.Run(state: Collation) = { state with locale = this.Locale }

    [<CustomOperation("case_level")>]
    member inline __.CaseLevel(state: Collation, caseLevel: bool) =
        { state with caseLevel = Some caseLevel }

    [<CustomOperation("case_first")>]
    member inline __.CaseFirst(state: Collation, caseFirst: bool) =
        { state with caseFirst = Some caseFirst }

    [<CustomOperation("strength")>]
    member inline __.Strength(state: Collation, strength: bool) = { state with strength = Some strength }

    [<CustomOperation("numeric_ordering")>]
    member inline __.NumericOrdering(state: Collation, numericOrdering: bool) =
        { state with numericOrdering = Some numericOrdering }

    [<CustomOperation("alternate")>]
    member inline __.Alternate(state: Collation, alternate: bool) =
        { state with alternate = Some alternate }

    [<CustomOperation("max_variable")>]
    member inline __.MaxVariable(state: Collation, maxVariable: bool) =
        { state with maxVariable = Some maxVariable }

    [<CustomOperation("backwards")>]
    member inline __.Backwards(state: Collation, backwards: bool) =
        { state with backwards = Some backwards }

type DeleteQueryBuilder() =
    member inline __.Yield _ =
        { q = Unchecked.defaultof<_> ()
          limit = 0
          collation = None
          hint = None
          comment = None }

    member inline __.Run(state: DeleteQuery<'Delete, 'Hint, 'Comment>) = state


    [<CustomOperation("query")>]
    member inline __.Query(state: DeleteQuery<'Delete, 'Hint, 'Comment>, query: 'Delete) = { state with q = query }

    [<CustomOperation("limit")>]
    member inline __.Limit(state: DeleteQuery<'Delete, 'Hint, 'Comment>, limit: int) = { state with limit = limit }

    [<CustomOperation("collation")>]
    member inline __.Collation(state: DeleteQuery<'Delete, 'Hint, 'Comment>, collation: Collation) =
        { state with collation = Some collation }

    [<CustomOperation("hint")>]
    member inline __.Hint(state: DeleteQuery<'Delete, 'Hint, 'Comment>, hint: 'Hint) = { state with hint = Some hint }

    [<CustomOperation("comment")>]
    member inline __.Comment(state: DeleteQuery<'Delete, 'Hint, 'Comment>, comment: 'Comment) =
        { state with comment = Some comment }

type UpdateQueryBuilder() =
    member inline __.Yield _ =
        { q = Unchecked.defaultof<_> ()
          u = Unchecked.defaultof<_> ()
          upsert = None
          multi = None
          collation = None
          arrayFilters = None
          hint = None }

    member inline __.Run(state: UpdateQuery<'Query, 'Update, 'Hint>) = state


    [<CustomOperation("query")>]
    member inline __.Query(state: UpdateQuery<'Query, 'Update, 'Hint>, query: 'Query) = { state with q = query }

    [<CustomOperation("update")>]
    member inline __.Update(state: UpdateQuery<'Query, 'Update, 'Hint>, update: 'Update) = { state with u = update }

    [<CustomOperation("upsert")>]
    member inline __.Upsert(state: UpdateQuery<'Query, 'Update, 'Hint>, upsert: bool) =
        { state with upsert = Some upsert }

    [<CustomOperation("multi")>]
    member inline __.Multi(state: UpdateQuery<'Query, 'Update, 'Hint>, multi: bool) = { state with multi = Some multi }

    [<CustomOperation("collation")>]
    member inline __.Collation(state: UpdateQuery<'Query, 'Update, 'Hint>, collation: Collation) =
        { state with collation = Some collation }


    [<CustomOperation("array_filters")>]
    member inline __.ArrayFilters(state: UpdateQuery<'Query, 'Update, 'Hint>, arrayFilters: obj seq) =
        { state with arrayFilters = Some arrayFilters }

    member this.ArrayFilters(state: UpdateQuery<'Query, 'Update, 'Hint>, arrayFilters: 'T seq) =
        this.ArrayFilters(state, arrayFilters |> Seq.map box)

    [<CustomOperation("hint")>]
    member inline __.Hint(state: UpdateQuery<'Query, 'Update, 'Hint>, hint: 'Hint) = { state with hint = hint }
