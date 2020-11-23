namespace Mondocks.Types

open MongoDB.Bson.Serialization.Attributes
open Mondocks
open System.Collections.Generic

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

[<BsonIgnoreExtraElementsAttribute>]
type Cursor<'T> = { firstBatch: seq<'T>; ns: string }

type FindResult<'T> = { cursor: Cursor<'T>; ok: float }

[<BsonIgnoreExtraElements>]
type InsertResult =
    { n: int
      ok: float }

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
