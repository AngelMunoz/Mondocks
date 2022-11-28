namespace Mondocks.Aggregation

open Mondocks.Types

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

type CountCommandBuilder(serialize: SerializerFn) =

    member val Serialize: SerializerFn = serialize

    member inline __.Yield _ =
        { count = ""
          query = None
          limit = None
          skip = None
          hint = None
          readConcern = None
          collation = None
          comment = None }

    member inline this.Run(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>) = this.Serialize state

    ///<summary>The name of the collection  to query against</summary>
    /// <example>
    ///    count {
    ///        collection "users"
    ///        query {| age = 50 |}
    ///    }
    /// </example>
    [<CustomOperation("collection")>]
    member inline __.Count(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, count: string) =
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
    member inline __.Query(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, query: 'TDocument) =
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
    member inline __.Limit(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, limit: int) =
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
    member inline __.Skip(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, skip: int) =
        { state with skip = Some skip }

    [<CustomOperation("hint")>]
    member inline __.Hint(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, hint: 'Hint) =
        { state with hint = Some hint }

    [<CustomOperation("collation")>]
    member inline __.Collation(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, collation: Collation) =
        { state with collation = Some collation }

    [<CustomOperation("comment")>]
    member inline __.Comment(state: CountCommand<'TDocument, 'Hint, 'ReadConcern, 'Comment>, comment: 'Comment) =
        { state with comment = Some comment }



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

type DistinctCommandBuilder(collection: string, serialize: SerializerFn) =

    member val Collection: string = collection

    member val Serialize: SerializerFn = serialize

    member inline __.Yield _ =
        { distinct = ""
          key = ""
          query = None
          readConcern = None
          collation = None
          comment = None }

    member inline this.Run(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>) =
        this.Serialize { state with distinct = this.Collection }

    /// <summary>
    /// A key used to retreive values from the query result documents
    /// </summary>
    /// <example>
    ///     distinct "users" {
    ///       key "email"
    ///     }
    /// </example>
    [<CustomOperation("key")>]
    member inline __.Key(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, key: string) =
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
    member inline __.Query(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, query: 'TDocument) =
        { state with query = Some query }

    [<CustomOperation("read_concern")>]
    member inline __.ReadConcern
        (
            state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>,
            readConcern: 'ReadConcern
        ) =
        { state with readConcern = Some readConcern }

    [<CustomOperation("collation")>]
    member inline __.Collation(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, collation: Collation) =
        { state with collation = Some collation }

    [<CustomOperation("collation")>]
    member inline __.Comment(state: DistinctCommand<'TDocument, 'ReadConcern, 'Comment>, comment: 'Comment) =
        { state with comment = Some comment }

type AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern> =
    { aggregate: obj
      pipeline: obj seq
      explain: Option<bool>
      allowDiskUse: Option<bool>
      cursor: Option<{| batchSize: int |}>
      maxTimeMS: Option<int>
      bypassDocumentValidation: Option<bool>
      readConcern: Option<'ReadConcern>
      collation: Option<Collation>
      hint: Option<'Hint>
      comment: Option<'Comment>
      writeConcern: Option<'WriteConcern> }

type AggregateCommandBuilder(aggregate: string, serialize: SerializerFn) =

    member val Aggregate: string = aggregate
    member val Serialize: SerializerFn = serialize

    member inline this.Yield _ =
        { aggregate = this.Aggregate
          pipeline = []
          explain = None
          allowDiskUse = None
          cursor = Some {| batchSize = 0 |}
          maxTimeMS = None
          bypassDocumentValidation = None
          readConcern = None
          collation = None
          hint = None
          comment = None
          writeConcern = None }

    member inline this.Run(state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>) = this.Serialize state

    /// <summary>
    /// An array of aggregation pipeline stages that process and transform the document stream as part of the aggregation pipeline.
    /// </summary>
    /// <example>
    ///     aggregate "users" {
    ///       // gets all of the distinc emails from users named "Frank"
    ///       pipeline [
    ///          box {| ``$project`` = {| Price = 1; Tag = 1 |} |}
    ///          box {| ``$group`` =
    ///                     {| _id = "$Tag";
    ///                        items = {| ``$sum`` = 1 |};
    ///                        total = {| ``$sum`` = "$Price" |}
    ///                     |}
    ///              |}
    ///       ]
    ///     }
    /// </example>
    [<CustomOperation("pipeline")>]
    member inline __.Pipeline
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            pipeline: obj seq
        ) =
        { state with pipeline = pipeline }

    /// <summary>
    /// Optional. Specifies to return the information on the processing of the pipeline.
    /// Not available in multi-document transactions.
    /// ***NOTE***: If you use this don't use <see cref="Mondocks.Types.FindResult">FindResult</see>
    /// use instead <see cref="MongoDB.Bson.BsonDocument">BsonDocument</see> as the result of RunCommand or RunCommandAsync
    /// since using explain will bring back a higly dynamic document
    /// </summary>
    [<CustomOperation("explain")>]
    member inline __.Explain(state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>, explain: bool) =
        { state with explain = Some explain }

    [<CustomOperation("allow_disk_use")>]
    member inline __.AllowDiskUse
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            allowDiskUse: bool
        ) =
        { state with allowDiskUse = Some allowDiskUse }

    [<CustomOperation("cursor_batch_size")>]
    member inline __.Cursor
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            cursorBatchSize: int
        ) =
        { state with cursor = Some {| batchSize = cursorBatchSize |} }

    [<CustomOperation("max_time_ms")>]
    member inline __.MaxtimeMs(state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>, maxTimeMs: int) =
        { state with maxTimeMS = Some maxTimeMs }

    [<CustomOperation("bypass_document_validation")>]
    member inline __.BypassDocumentValidation
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            bypassDocumentValidation: bool
        ) =
        { state with bypassDocumentValidation = Some bypassDocumentValidation }

    [<CustomOperation("read_concern")>]
    member inline __.ReadConcern
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            readConcern: 'ReadConcern
        ) =
        { state with readConcern = Some readConcern }

    [<CustomOperation("collation")>]
    member inline __.Collation
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            collation: Collation
        ) =
        { state with collation = Some collation }

    [<CustomOperation("hint")>]
    member inline __.Hint(state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>, hint: 'Hint) =
        { state with hint = Some hint }

    [<CustomOperation("comment")>]
    member inline __.Comment(state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>, comment: 'Comment) =
        { state with comment = Some comment }

    [<CustomOperation("write_concern")>]
    member inline __.WriteConcern
        (
            state: AggregateCommand<'ReadConcern, 'Hint, 'Comment, 'WriteConcern>,
            writeConcern: 'WriteConcern
        ) =
        { state with writeConcern = Some writeConcern }
