namespace Mondocks.Queries

open Mondocks.Types

/// <summary>
/// Represents a Find command
/// for more information take a look at <a href="https://docs.mongodb.com/manual/reference/command/find/#syntax">find syntax</a>
/// </summary>
type FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min> =
    {
      /// Name of the collection where this find operation will be targeted
      find: string
      /// Optional object that is going to be used to filter
      filter: Option<'Filter>
      /// <summary>
      ///  optional object to sort by property
      /// </summary>
      /// <example>
      ///     Some {| ``$sort`` = {| name = 1; age = -1 |} |}
      /// </example>
      /// <example>
      ///     None
      /// </example>
      sort: Option<'Sort>
      /// <summary>A <a href="https://docs.mongodb.com/manual/reference/method/db.collection.find/#find-projection">projection object</a></summary>
      projection: Option<'Projection>
      hint: Option<'Hint>
      /// User for pagination
      skip: Option<int>
      /// User for pagination
      limit: Option<int>
      batchSize: Option<int>
      singleBatch: Option<bool>
      comment: Option<'Comment>
      maxTimeMS: Option<int>
      readConcern: Option<'ReadConcern>
      max: Option<'Max>
      min: Option<'Min>
      returnKey: Option<bool>
      showRecordId: Option<bool>
      tailable: Option<bool>
      oplogReplay: Option<bool>
      noCursorTimeout: Option<bool>
      awaitData: Option<bool>
      allowPartialResults: Option<bool>
      collation: Option<Collation>
      allowDiskUse: Option<bool> }



/// <summary>
///   Tries to find a single document and apply the supplied modifications to it
///   <a href="https://docs.mongodb.com/manual/reference/command/findAndModify/#findandmodify">Reference</a>
/// </summary>
type FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment> =
    { /// name of the collection where this query is targeted at
      findAndModify: string
      /// An optional object that will be used as a filter for the query
      query: Option<'Query>
      /// <summary>
      ///  optional object to sort by property
      /// </summary>
      /// <example>
      ///    Some {| ``$sort`` = {| name = 1; age = -1 |} |}
      /// </example>
      /// <example>
      ///    None
      /// </example>
      sort: Option<'Sort>
      /// Optional property that indicates whether this document should be removed or not.
      /// Must specify either the remove or the update field
      remove: Option<bool>
      /// Optional property that indicates the new values for the objects that match the query.
      /// Must specify either the remove or the update field
      update: Option<'Update>
      /// optional
      /// When true, returns the modified document rather than the original
      ``new``: Option<bool>
      /// <summary>
      ///  A subset of fields to return. The fields document specifies an inclusion of a field with 1, as in
      /// </summary>
      /// <example>
      ///    Some {| name = 1; password = 0 |}
      /// </example>
      /// <example>
      ///    None
      /// </example>
      fields: Option<'Fields>
      /// <summary>
      /// Optional.
      /// Used in conjunction with the update field.
      /// Creates a new document if no documents match the query
      /// Updates a single document that matches the query.
      /// </summary>
      upsert: Option<bool>
      bypassDocumentValidation: Option<bool>
      writeConcern: Option<'WriteConcern>
      collation: Option<Collation>
      arrayFilters: Option<seq<obj>>
      hint: Option<'Hint>
      comment: Option<'Comment> }


type FindBuilder(collection: string, serialize: SerializerFn) =

    member __.Yield _ =
        { find = ""
          filter = None
          sort = None
          projection = None
          hint = None
          skip = None
          limit = None
          batchSize = None
          singleBatch = None
          comment = None
          maxTimeMS = None
          readConcern = None
          max = None
          min = None
          returnKey = None
          showRecordId = None
          tailable = None
          oplogReplay = None
          noCursorTimeout = None
          awaitData = None
          allowPartialResults = None
          collation = None
          allowDiskUse = None }


    /// <summary>
    /// Converts the query into a serialized string
    /// </summary>
    member __.Run(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>) =
        serialize { state with find = collection }

    /// <summary>
    ///  Optional object that is going to be used to filter the find query
    /// </summary>
    /// <example>
    ///     find "user" {
    ///       // filters by name equals Frank
    ///       filter {| name = "Frank"|}
    ///     }
    /// </example>
    [<CustomOperation("filter")>]
    member __.WithFilter
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            filter: 'Filter
        ) =
        { state with filter = Some filter }

    /// <summary>
    ///  Optional. Object to sort by property
    /// </summary>
    /// <example>
    ///     find "user" {
    ///       // ... omit other fields
    ///       // sorts by name ascending and age descending
    ///       sort {| ``$sort`` = {| name = 1; age = -1 |} |}
    ///       // ... omit other fields
    ///     }
    /// </example>
    [<CustomOperation("sort")>]
    member __.WithSort
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            sort: 'Sort
        ) =
        { state with sort = Some sort }

    /// <summary>
    ///  A <a href="https://docs.mongodb.com/manual/reference/method/db.collection.find/#find-projection">Projection Object</a>
    /// </summary>
    /// <example>
    ///     find "user" {
    ///       // ... omit other fields
    ///       // from the found documents bring only the name and the age, omit the password
    ///       projection {| name = 1; age = 1; password = 0 |}
    ///       // ... omit other fields
    ///     }
    /// </example>
    [<CustomOperation("projection")>]
    member __.WithProjection
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            projection: 'Projection
        ) =
        { state with
              projection = Some projection }

    [<CustomOperation("hint")>]
    member __.WithHint
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            hint: 'Hint
        ) =
        { state with hint = Some hint }

    /// <summary>
    /// Optional. An integer value used to omit the first n amount of documents from the find query
    /// Use together with Limit to paginate results
    /// </summary>
    /// <example>
    ///     find "user" {
    ///       // ... omit other fields
    ///       // omits the first 10 results of the find query
    ///       skip 10
    ///       // ... omit other fields
    ///     }
    /// </example>
    [<CustomOperation("skip")>]
    member __.WithSkip
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            skip: int
        ) =
        { state with skip = Some skip }

    /// <summary>
    /// Optional. An integer value used to take at most n amount of documents
    /// Use together with Skip to paginate results
    /// </summary>
    /// <example>
    ///     find "user" {
    ///       // ... omit other fields
    ///       // takes only 10 documents from the find query
    ///       limit 10
    ///       // ... omit other fields
    ///     }
    /// </example>
    [<CustomOperation("limit")>]
    member __.WithLimit
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            limit: int
        ) =
        { state with limit = Some limit }

    [<CustomOperation("batch_size")>]
    member __.WithBatchSize
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            batch_size: int
        ) =
        { state with
              batchSize = Some batch_size }

    [<CustomOperation("single_batch")>]
    member __.WithSingleBatch
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            singleBatch: bool
        ) =
        { state with
              singleBatch = Some singleBatch }

    [<CustomOperation("comment")>]
    member __.WithComment
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            comment: 'Comment
        ) =
        { state with comment = Some comment }

    [<CustomOperation("max_time_ms")>]
    member __.WithMaxTimeMS
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            maxTimeMs: int
        ) =
        { state with
              maxTimeMS = Some maxTimeMs }

    [<CustomOperation("read_concern")>]
    member __.WithReadConcern
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            readConcern: 'ReadConcern
        ) =
        { state with
              readConcern = Some readConcern }

    [<CustomOperation("max")>]
    member __.WithMax
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            max: 'Max
        ) =
        { state with max = Some max }

    [<CustomOperation("min")>]
    member __.WithMin
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            min: 'Min
        ) =
        { state with min = Some min }

    [<CustomOperation("return_key")>]
    member __.WithReturnKey
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            returnKey: bool
        ) =
        { state with
              returnKey = Some returnKey }

    [<CustomOperation("show_record_id")>]
    member __.WithShowRecordId
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            showRecordId: bool
        ) =
        { state with
              showRecordId = Some showRecordId }

    [<CustomOperation("tailable")>]
    member __.WithTailable
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            tailable: bool
        ) =
        { state with tailable = Some tailable }

    [<CustomOperation("oplog_replay")>]
    member __.WithOplogReplay
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            oplogReplay: bool
        ) =
        { state with
              oplogReplay = Some oplogReplay }

    [<CustomOperation("no_cursor_timeout")>]
    member __.WithNoCursorTimeout
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            noCursorTimeOut: bool
        ) =
        { state with
              noCursorTimeout = Some noCursorTimeOut }

    [<CustomOperation("await_data")>]
    member __.WithAwaitData
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            awaitData: bool
        ) =
        { state with
              awaitData = Some awaitData }

    [<CustomOperation("allow_partial_results")>]
    member __.WithAllowPartialResults
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            awaitData: bool
        ) =
        { state with
              allowPartialResults = Some awaitData }

    [<CustomOperation("collation")>]
    member __.WithCollation
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            collation: Collation
        ) =
        { state with
              collation = Some collation }

    [<CustomOperation("allow_disk_use")>]
    member __.WithAllowDiskUse
        (
            state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
            allowDiskUse: bool
        ) =
        { state with
              allowDiskUse = Some allowDiskUse }

type FindAndModifyBuilder(collection: string, serialize: SerializerFn) =
    member __.Yield _ =
        { findAndModify = ""
          query = None
          sort = None
          remove = None
          update = None
          ``new`` = None
          fields = None
          upsert = None
          bypassDocumentValidation = None
          writeConcern = None
          collation = None
          arrayFilters = None
          hint = None
          comment = None }

    /// <summary>
    /// Converts the query into a serialized string
    /// </summary>
    member __.Run(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>) =
        serialize
            { state with
                  findAndModify = collection }

    /// <summary>
    ///  Optional object that is going to be used to filter the findAndModify query
    /// </summary>
    /// <example>
    ///     findAndModify "user" {
    ///       // filters the collection for names that are either
    ///       // Frank, Sandra, Peter or Monica
    ///       query {| name = {| ``$in`` = ["Frank"; "Sandra"; "Peter"; "Monica"] |}
    ///     }
    /// </example>
    /// <example>
    ///     findAndModify "user" {
    ///       // filters by age where the age is greater or equal than 30
    ///       query {| age = {| ``$gte`` = 30 |}
    ///     }
    /// </example>
    [<CustomOperation("query")>]
    member __.WithQuery
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            query: 'Query
        ) =
        { state with query = Some query }


    /// <summary>
    ///  A <a href="https://docs.mongodb.com/manual/reference/method/db.collection.find/#find-projection">Projection Object</a>
    /// </summary>
    /// <example>
    ///     findAndModify "user" {
    ///       // ... omit other fields
    ///       // sorts by name ascending and age descending
    ///       sort {| ``$sort`` = {| name = 1; age = -1 |} |}
    ///       // ... omit other fields
    ///     }
    /// </example>
    [<CustomOperation("sort")>]
    member __.WithSort
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            sort: 'Sort
        ) =
        { state with sort = Some sort }
    /// Optional. property that indicates whether this document should be removed or not.
    /// Must specify either the remove or the update field
    [<CustomOperation("remove")>]
    member __.WithRemove
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            remove: bool
        ) =
        { state with remove = Some remove }
    /// Optional. property that indicates the new values for the objects that match the query.
    /// Must specify either the remove or the update field
    [<CustomOperation("update")>]
    member __.WithUpdate
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            update: 'Update
        ) =
        { state with update = Some update }

    [<CustomOperation("new")>]
    member __.WithNew
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            withNew: bool
        ) =
        { state with ``new`` = Some withNew }

    /// <summary>
    ///  A <a href="https://docs.mongodb.com/manual/reference/method/db.collection.find/#find-projection">Projection Object</a>.
    ///  It is a subset of fields to return. The fields document specifies an inclusion of a field with 1
    /// </summary>
    /// <example>
    ///     findAndModify "user" {
    ///       // ... omit other fields
    ///       // from the found documents bring only the name and the age, omit the password
    ///       fields {| name = 1; age = 1; password = 0 |}
    ///       // ... omit other fields
    ///     }
    /// </example>
    [<CustomOperation("fields")>]
    member __.WithFields
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            fields: 'Fields
        ) =
        { state with fields = Some fields }

    /// <summary>
    /// Optional.
    /// Used in conjunction with the update field.
    /// Creates a new document if no documents match the query
    /// Updates a single document that matches the query.
    /// </summary>
    [<CustomOperation("upsert")>]
    member __.WithUpsert
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            upsert: bool
        ) =
        { state with upsert = Some upsert }

    [<CustomOperation("bypass_document_validation")>]
    member __.WithBypassDocumentValidation
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            bypassDocumentValidation: bool
        ) =
        { state with
              bypassDocumentValidation = Some bypassDocumentValidation }

    [<CustomOperation("write_concern")>]
    member __.WithWriteConcern
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            writeConcern: 'WriteConcern
        ) =
        { state with
              writeConcern = Some writeConcern }

    [<CustomOperation("collation")>]
    member __.WithCollation
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            collation: Collation
        ) =
        { state with
              collation = Some collation }


    [<CustomOperation("array_filters")>]
    member __.WithArrayFilters
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            arrayFilters: seq<obj>
        ) =
        { state with
              arrayFilters = Some arrayFilters }

    [<CustomOperation("hint")>]
    member __.WithHint
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            hint: 'Hint
        ) =
        { state with hint = Some hint }

    [<CustomOperation("comment")>]
    member __.WithComment
        (
            state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
            comment: 'Comment
        ) =
        { state with comment = Some comment }

type DeleteCommand<'WriteConcern> =
    { delete: string
      /// criteria queries to match against the database
      deletes: seq<obj>
      ordered: Option<bool>
      writeConcern: Option<'WriteConcern> }


type DeleteBuilder(collection: string, serialize: SerializerFn) =
    member __.Yield _ =
        { delete = ""
          deletes = Seq.empty
          ordered = None
          writeConcern = None }

    member __.Run(state: DeleteCommand<'WriteConcern>) =
        serialize { state with delete = collection }

    /// <summary>
    ///  A sequence of anonymous record objects (boxed) that will be used
    /// to perform against the collection
    /// </summary>
    /// <example>
    ///     delete "users" {
    ///       // deletes 1 user named "Peter"
    ///       deletes [box {| q = {| name = "Peter" |}; limit = 1 |}]
    ///     }
    /// </example>
    [<CustomOperation("deletes")>]
    member __.WithDeletes(state: DeleteCommand<'WriteConcern>, deletes: seq<obj>) = { state with deletes = deletes }

    /// <summary>
    ///  A sequence of <see cref="Mondocks.Types.DeleteQuery">DeleteQuery</see> objects that will be used
    /// to perform against the collection
    /// </summary>
    /// <example>
    ///     delete "users" {
    ///       // filters the collection for names that are either
    ///       // Frank, Sandra, Peter or Monica
    ///       deletes [
    ///           { q = {| name = name |}
    ///             // To delete all documents that match
    ///             // `limit = 0`
    ///             limit = 1
    ///             collation = None
    ///             hint = None
    ///             comment = None }
    ///       ]
    ///     }
    /// </example>
    member this.WithDeletes(state: DeleteCommand<'WriteConcern>, deletes: seq<DeleteQuery<'Delete, 'Hint, 'Comment>>) =
        this.WithDeletes(state, deletes |> Seq.map box)

    [<CustomOperation("ordered")>]
    member __.WithOrdered(state: DeleteCommand<'WriteConcern>, ordered: bool) = { state with ordered = Some ordered }

    [<CustomOperation("write_concern")>]
    member __.WithWriteConcern(state: DeleteCommand<'WriteConcern>, concern: 'WriteConcern) =
        { state with
              writeConcern = Some concern }

type InsertCommand<'TDocument, 'WriteConcern, 'Comment> =
    { insert: string
      documents: seq<'TDocument>
      ordered: Option<bool>
      writeConcern: Option<'WriteConcern>
      bypassDocumentValidation: Option<bool>
      comment: Option<'Comment> }

type InsertCommandBuilder(collection: string, serialize: SerializerFn) =

    member __.Yield _ =
        { insert = ""
          documents = Seq.empty
          ordered = None
          writeConcern = None
          bypassDocumentValidation = None
          comment = None }

    member __.Run(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>) =
        serialize { state with insert = collection }

    /// <summary>
    ///  A sequence of documents that will be inserted
    /// into the collection, they can be records or anonymous records
    /// </summary>
    /// <example>
    ///     type Post = { title: string; content: string }
    ///     insert "posts" {
    ///       // Inserts two new documents
    ///       documents
    ///           [ { title= "Title"; content = "Content" }
    ///             { title= "Title"; content = "Content" } ]
    ///     }
    /// </example>
    /// <example>
    ///     insert "posts" {
    ///       // Inserts two new documents
    ///       documents
    ///           [ {| title= "Title"; content = "Content" |}
    ///             {| title= "Title"; content = "Content" |} ]
    ///     }
    /// </example>
    [<CustomOperation("documents")>]
    member __.WithDocuments(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, documents: seq<'TDocument>) =
        { state with documents = documents }

    [<CustomOperation("ordered")>]
    member __.WithOrdered(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, ordered: bool) =
        { state with ordered = Some ordered }

    [<CustomOperation("write_concern")>]
    member __.WithWriteConcern(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, writeConcern: 'WriteConcern) =
        { state with
              writeConcern = Some writeConcern }

    [<CustomOperation("bypass_document_validation")>]
    member __.WithBypassDocumentValidation
        (
            state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>,
            bypassDocumentValidation: bool
        ) =
        { state with
              bypassDocumentValidation = Some bypassDocumentValidation }

    [<CustomOperation("comment")>]
    member __.WithComment(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, comment: 'Comment) =
        { state with comment = Some comment }

type UpdateCommand<'WriteConcern, 'Comment> =
    { update: string
      updates: seq<obj>
      ordered: Option<bool>
      writeConcern: Option<'WriteConcern>
      bypassDocumentValidation: Option<bool>
      comment: Option<'Comment> }

type UpdateCommandBuilder(collection: string, serialize: SerializerFn) =

    member __.Yield _ =
        { update = ""
          updates = Seq.empty
          ordered = None
          writeConcern = None
          bypassDocumentValidation = None
          comment = None }

    member __.Run(state: UpdateCommand<'WriteConcern, 'Comment>) =
        serialize { state with update = collection }


    /// <summary>
    ///  A sequence of custom documents that will be used to query against the collection
    /// </summary>
    /// <example>
    ///     update "users" {
    ///       // Inserts two new documents
    ///       updates
    ///           [ box {| q = {| _id = user._id |}
    ///                    u = {| user with email = "new@email.com" |}
    ///                    upsert = true
    ///                    multi = false |} ]
    ///     }
    /// </example>
    [<CustomOperation("updates")>]
    member __.WithUpdates(state: UpdateCommand<'WriteConcern, 'Comment>, updates: seq<obj>) =
        { state with updates = updates }

    /// <summary>
    ///  A sequence of custom documents that will be used to query against the collection
    /// </summary>
    /// <example>
    ///     update "users" {
    ///       // Inserts two new documents
    ///       updates
    ///           [ { q = {| _id = user._id |}
    ///               u = { user with email = "new@email.com" }
    ///               upsert = Some false
    ///               multi = Some false
    ///               collation = None
    ///               arrayFilters = None
    ///               hint = None } ]
    ///     }
    /// </example>
    member this.WithUpdates
        (
            state: UpdateCommand<'WriteConcern, 'Comment>,
            updates: seq<UpdateQuery<'Query, 'Update, 'Hint>>
        ) =
        this.WithUpdates(state, updates |> Seq.map box)

    [<CustomOperation("ordered")>]
    member __.WithOrdered(state: UpdateCommand<'WriteConcern, 'Comment>, ordered: bool) =
        { state with ordered = Some ordered }

    [<CustomOperation("write_concern")>]
    member __.WithWriteConcern(state: UpdateCommand<'WriteConcern, 'Comment>, writeConcern: 'WriteConcern) =
        { state with
              writeConcern = Some writeConcern }

    [<CustomOperation("bypass_document_validation")>]
    member __.WithBypassDocumentValidation
        (
            state: UpdateCommand<'WriteConcern, 'Comment>,
            bypassDocumentValidation: bool
        ) =
        { state with
              bypassDocumentValidation = Some bypassDocumentValidation }

    [<CustomOperation("comment")>]
    member __.WithComment(state: UpdateCommand<'WriteConcern, 'Comment>, comment: 'Comment) =
        { state with comment = Some comment }
