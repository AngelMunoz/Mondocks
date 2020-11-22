namespace Mondocks

open Mondocks.Types


module Query =
    open System

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

        type FindBuilder() =

            member __.Zero value = __.Return value

            member __.Return value = __.Yield value

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

        let find = FindBuilder()
