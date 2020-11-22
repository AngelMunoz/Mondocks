namespace Mondocks.Types

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
    { /// that represents the matching criteria
      q: 'Delete
      /// amount of documents to delete. use `0` to delete all matching criteria
      limit: int
      collation: Option<Collation>
      hint: Option<'Hint>
      comment: Option<'Comment> }


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
