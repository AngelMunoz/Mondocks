namespace Mondocks

open Mondocks.Types

module Queries =

    [<AutoOpen>]
    module Find =
        type FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min> =
            { find: string
              filter: Option<'Filter>
              sort: Option<'Sort>
              projection: Option<'Projection>
              hint: Option<'Hint>
              skip: Option<int>
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

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment> =
            { findAndModify: string
              query: Option<'Query>
              sort: Option<'Sort>
              remove: Option<bool>
              update: Option<'Update>
              ``new``: Option<bool>
              fields: Option<'Fields>
              upsert: Option<bool>
              bypassDocumentValidation: Option<bool>
              writeConcern: Option<'WriteConcern>
              collation: Option<Collation>
              arrayFilters: Option<seq<obj>>
              hint: Option<'Hint>
              comment: Option<'Comment> }
            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type FindBuilder() =

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

            member __.Run(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>) =
                (state :> IBuilder).ToJSON()

            [<CustomOperation("use_collection")>]
            member __.UseCollection(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                    name: string) =
                { state with find = name }

            [<CustomOperation("with_filter")>]
            member __.WithFilter(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                 filter: 'Filter) =
                { state with filter = Some filter }

            [<CustomOperation("with_sort")>]
            member __.WithSort(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                               sort: 'Sort) =
                { state with sort = Some sort }

            [<CustomOperation("with_projection")>]
            member __.WithProjection(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                     projection: 'Projection) =
                { state with
                      projection = Some projection }

            [<CustomOperation("with_hint")>]
            member __.WithHint(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                               hint: 'Hint) =
                { state with hint = Some hint }

            [<CustomOperation("with_skip")>]
            member __.WithSkip(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                               skip: int) =
                { state with skip = Some skip }

            [<CustomOperation("with_limit")>]
            member __.WithLimit(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                limit: int) =
                { state with limit = Some limit }

            [<CustomOperation("with_batch_size")>]
            member __.WithBatchSize(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                    batch_size: int) =
                { state with
                      batchSize = Some batch_size }

            [<CustomOperation("with_single_batch")>]
            member __.WithSingleBatch(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                      singleBatch: bool) =
                { state with
                      singleBatch = Some singleBatch }

            [<CustomOperation("with_comment")>]
            member __.WithComment(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                  comment: 'Comment) =
                { state with comment = Some comment }

            [<CustomOperation("with_max_time_ms")>]
            member __.WithMaxTimeMS(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                    maxTimeMs: int) =
                { state with
                      maxTimeMS = Some maxTimeMs }

            [<CustomOperation("with_read_concern")>]
            member __.WithReadConcern(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                      readConcern: 'ReadConcern) =
                { state with
                      readConcern = Some readConcern }

            [<CustomOperation("with_max")>]
            member __.WithMax(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                              max: 'Max) =
                { state with max = Some max }

            [<CustomOperation("with_min")>]
            member __.WithMin(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                              min: 'Min) =
                { state with min = Some min }

            [<CustomOperation("with_return_key")>]
            member __.WithReturnKey(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                    returnKey: bool) =
                { state with
                      returnKey = Some returnKey }

            [<CustomOperation("with_show_record_id")>]
            member __.WithShowRecordId(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                       showRecordId: bool) =
                { state with
                      showRecordId = Some showRecordId }

            [<CustomOperation("with_tailable")>]
            member __.WithTailable(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                   tailable: bool) =
                { state with tailable = Some tailable }

            [<CustomOperation("with_oplog_replay")>]
            member __.WithOplogReplay(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                      oplogReplay: bool) =
                { state with
                      oplogReplay = Some oplogReplay }

            [<CustomOperation("with_no_cursor_timeout")>]
            member __.WithNoCursorTimeout(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                          noCursorTimeOut: bool) =
                { state with
                      noCursorTimeout = Some noCursorTimeOut }

            [<CustomOperation("with_await_data")>]
            member __.WithAwaitData(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                    awaitData: bool) =
                { state with
                      awaitData = Some awaitData }

            [<CustomOperation("with_allow_partial_results")>]
            member __.WithAllowPartialResults(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                              awaitData: bool) =
                { state with
                      allowPartialResults = Some awaitData }

            [<CustomOperation("with_collation")>]
            member __.WithCollation(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                    collation: Collation) =
                { state with
                      collation = Some collation }

            [<CustomOperation("with_allow_disk_use")>]
            member __.WithAllowDiskUse(state: FindCommand<'Filter, 'Sort, 'Projection, 'Hint, 'Comment, 'ReadConcern, 'Max, 'Min>,
                                       allowDiskUse: bool) =
                { state with
                      allowDiskUse = Some allowDiskUse }

        type FindAndModifyBuilder() =


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

            member __.Run(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>)
                         =
                (state :> IBuilder).ToJSON()

            [<CustomOperation("use_collection")>]
            member __.UseCollection(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                    collection: string) =
                { state with
                      findAndModify = collection }

            [<CustomOperation("with_query")>]
            member __.WithQuery(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                query: 'Query) =
                { state with query = Some query }

            [<CustomOperation("with_sort")>]
            member __.WithSort(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                               sort: 'Sort) =
                { state with sort = Some sort }

            [<CustomOperation("with_remove")>]
            member __.WithRemove(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                 remove: bool) =
                { state with remove = Some remove }

            [<CustomOperation("with_update")>]
            member __.WithUpdate(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                 update: 'Update) =
                { state with update = Some update }

            [<CustomOperation("with_new")>]
            member __.WithNew(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                              withNew: bool) =
                { state with ``new`` = Some withNew }

            [<CustomOperation("with_fields")>]
            member __.WithFields(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                 fields: 'Fields) =
                { state with fields = Some fields }

            [<CustomOperation("with_upsert")>]
            member __.WithUpsert(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                 upsert: bool) =
                { state with upsert = Some upsert }

            [<CustomOperation("with_bypass_document_validation")>]
            member __.WithBypassDocumentValidation(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                                   bypassDocumentValidation: bool) =
                { state with
                      bypassDocumentValidation = Some bypassDocumentValidation }

            [<CustomOperation("with_write_concern")>]
            member __.WithWriteConcern(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                       writeConcern: 'WriteConcern) =
                { state with
                      writeConcern = Some writeConcern }

            [<CustomOperation("with_collation")>]
            member __.WithCollation(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                    collation: Collation) =
                { state with
                      collation = Some collation }


            [<CustomOperation("with_array_filters")>]
            member __.WithArrayFilters(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                       arrayFilters: seq<obj>) =
                { state with
                      arrayFilters = Some arrayFilters }

            [<CustomOperation("with_hint")>]
            member __.WithHint(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                               hint: 'Hint) =
                { state with hint = Some hint }

            [<CustomOperation("with_comment")>]
            member __.WithComment(state: FindAndModifyCommand<'Query, 'Sort, 'Update, 'Fields, 'WriteConcern, 'Hint, 'Comment>,
                                  comment: 'Comment) =
                { state with comment = Some comment }


        let findAndModify = FindAndModifyBuilder()
        let find = FindBuilder()

    [<AutoOpen>]
    module Delete =
        type DeleteCommand<'WriteConcern> =
            { delete: string
              /// criteria queries to match against the database
              deletes: seq<obj>
              ordered: Option<bool>
              writeConcern: Option<'WriteConcern> }
            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type DeleteBuilder() =

            member __.Yield _ =
                { delete = ""
                  deletes = Seq.empty
                  ordered = None
                  writeConcern = None }

            member __.Run(state: DeleteCommand<'WriteConcern>) = (state :> IBuilder).ToJSON()

            [<CustomOperation("use_collection")>]
            member __.UseCollection(state: DeleteCommand<'WriteConcern>, collection: string) =
                { state with delete = collection }

            [<CustomOperation("with_deletes")>]
            member __.WithDeletes(state: DeleteCommand<'WriteConcern>,
                                  deletes: seq<DeleteQuery<'Delete, 'Hint, 'Comment>>) =
                { state with
                      deletes = deletes |> Seq.map box }

            [<CustomOperation("with_ordered")>]
            member __.WithOrdered(state: DeleteCommand<'WriteConcern>, ordered: bool) =
                { state with ordered = Some ordered }

            [<CustomOperation("with_write_concern")>]
            member __.WithWriteConcern(state: DeleteCommand<'WriteConcern>, concern: 'WriteConcern) =
                { state with
                      writeConcern = Some concern }

        let delete = DeleteBuilder()


    [<AutoOpen>]
    module Insert =
        type InsertCommand<'TDocument, 'WriteConcern, 'Comment> =
            { insert: string
              documents: seq<'TDocument>
              ordered: Option<bool>
              writeConcern: Option<'WriteConcern>
              bypassDocumentValidation: Option<bool>
              comment: Option<'Comment> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type InsertCommandBuilder() =

            member __.Yield(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>) =
                { insert = ""
                  documents = Seq.empty
                  ordered = None
                  writeConcern = None
                  bypassDocumentValidation = None
                  comment = None }

            member __.Run(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>) = (state :> IBuilder).ToJSON()

            [<CustomOperation("use_collection")>]
            member __.UseCollection(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, collection: string) =
                { state with insert = collection }

            [<CustomOperation("with_documents")>]
            member __.WithDocuments(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>,
                                    documents: seq<'TDocument>) =
                { state with documents = documents }

            [<CustomOperation("with_ordered")>]
            member __.WithOrdered(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, ordered: bool) =
                { state with ordered = Some ordered }

            [<CustomOperation("with_write_concern")>]
            member __.WithWriteConcern(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>,
                                       writeConcern: 'WriteConcern) =
                { state with
                      writeConcern = Some writeConcern }

            [<CustomOperation("with_bypass_document_validation")>]
            member __.WithBypassDocumentValidation(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>,
                                                   bypassDocumentValidation: bool) =
                { state with
                      bypassDocumentValidation = Some bypassDocumentValidation }

            [<CustomOperation("with_comment")>]
            member __.WithComment(state: InsertCommand<'TDocument, 'WriteConcern, 'Comment>, comment: 'Comment) =
                { state with comment = Some comment }

        let insert = InsertCommandBuilder()

    [<AutoOpen>]
    module Update =
        type UpdateCommand<'WriteConcern, 'Comment> =
            { update: string
              updates: seq<obj>
              ordered: Option<bool>
              writeConcern: Option<'WriteConcern>
              bypassDocumentValidation: Option<bool>
              comment: Option<'Comment> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type UpdateCommandBuilder() =

            member __.Yield(state: UpdateCommand<'WriteConcern, 'Comment>) =
                { update = ""
                  updates = Seq.empty
                  ordered = None
                  writeConcern = None
                  bypassDocumentValidation = None
                  comment = None }

            member __.Run(state: UpdateCommand<'WriteConcern, 'Comment>) = (state :> IBuilder).ToJSON()

            [<CustomOperation("use_collection")>]
            member __.UseCollection(state: UpdateCommand<'WriteConcern, 'Comment>, collection: string) =
                { state with update = collection }

            [<CustomOperation("with_updates")>]
            member __.WithUpdates(state: UpdateCommand<'WriteConcern, 'Comment>,
                                  updates: seq<UpdateQuery<'Query, 'Update, 'Hint>>) =
                { state with
                      updates = updates |> Seq.map box }

            [<CustomOperation("with_ordered")>]
            member __.WithOrdered(state: UpdateCommand<'WriteConcern, 'Comment>, ordered: bool) =
                { state with ordered = Some ordered }

            [<CustomOperation("with_write_concern")>]
            member __.WithWriteConcern(state: UpdateCommand<'WriteConcern, 'Comment>, writeConcern: 'WriteConcern) =
                { state with
                      writeConcern = Some writeConcern }

            [<CustomOperation("with_bypass_document_validation")>]
            member __.WithBypassDocumentValidation(state: UpdateCommand<'WriteConcern, 'Comment>,
                                                   bypassDocumentValidation: bool) =
                { state with
                      bypassDocumentValidation = Some bypassDocumentValidation }

            [<CustomOperation("with_comment")>]
            member __.WithComment(state: UpdateCommand<'WriteConcern, 'Comment>, comment: 'Comment) =
                { state with comment = Some comment }

        let update = UpdateCommandBuilder()
