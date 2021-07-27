namespace Mondocks.Types

open MongoDB.Bson.Serialization.Attributes

[<BsonIgnoreExtraElements>]
type Cursor<'T> = { firstBatch: seq<'T>; ns: string }

type FindResult<'T> = { cursor: Cursor<'T>; ok: float }

[<BsonIgnoreExtraElements>]
type InsertResult = { n: int; ok: float }


[<BsonIgnoreExtraElements>]
type CreateIndexResult =
    { createdCollectionAutomatically: bool
      numIndexesBefore: int
      numIndexesAfter: int
      ok: float }

[<BsonIgnoreExtraElements>]
type DropIndexResult = { nIndexesWas: int; ok: float }
