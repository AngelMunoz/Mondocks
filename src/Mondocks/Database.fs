namespace Mondocks

open System
open System.Collections.Generic
open Mondocks.Types

module Database =

    [<AutoOpen>]
    module Administration =

        type DropIndexCommand<'WriteConcern, 'Comment> =
            { dropIndexes: string
              index: obj
              writeConcern: Option<'WriteConcern>
              comment: Option<'Comment> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type CreateIndexesCommand<'Index, 'WriteConcern, 'CommitQuorum, 'Comment> =
            { createIndexes: string
              indexes: seq<obj>
              writeConcern: Option<'WriteConcern>
              commitQuorum: Option<'CommitQuorum>
              comment: Option<'Comment> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection> =
            { name: string
              key: IDictionary<string, obj>
              background: Option<bool>
              unique: Option<bool>
              partialFilterExpression: Option<'PartialFilterExpression>
              sparse: Option<bool>
              expireAfterSeconds: Option<int>
              hidden: Option<bool>
              storageEngine: Option<'StorageEngine>
              weights: Option<'Weights>
              default_language: Option<string>
              language_override: Option<string>
              textIndexVersion: Option<int>
              ``2dsphereIndexVersion``: Option<int>
              bits: Option<int>
              min: Option<float>
              max: Option<float>
              bucketSize: Option<float>
              collation: Option<Collation>
              wildcardProjection: Option<'WildcardProjection> }

            interface IBuilder with
                member __.ToJSON() = Json.Serialize __

        type IndexBuilder(name: string) =

            member __.Yield _ =
                { name = ""
                  key = dict ([])
                  background = None
                  unique = None
                  partialFilterExpression = None
                  sparse = None
                  expireAfterSeconds = None
                  hidden = None
                  storageEngine = None
                  weights = None
                  default_language = None
                  language_override = None
                  textIndexVersion = None
                  ``2dsphereIndexVersion`` = None
                  bits = None
                  min = None
                  max = None
                  bucketSize = None
                  collation = None
                  wildcardProjection = None }

            member __.Run(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>) =
                ({ state with name = name })

            [<CustomOperation("key")>]
            member __.Key(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                          key: IDictionary<string, obj>) =
                { state with key = key }

            [<CustomOperation("background")>]
            member __.Background(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                 background: bool) =
                { state with
                      background = Some background }

            [<CustomOperation("unique")>]
            member __.Unique(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                             unique: bool) =
                { state with unique = Some unique }

            [<CustomOperation("partial_filter_expression")>]
            member __.PartialFilterExpression(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                              partialFilterExpression: 'PartialFilterExpression) =
                { state with
                      partialFilterExpression = Some partialFilterExpression }

            [<CustomOperation("sparse")>]
            member __.Sparse(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                             sparse: bool) =
                { state with sparse = Some sparse }

            [<CustomOperation("expire_after_seconds")>]
            member __.ExpireAfterSeconds(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                         expireAfterSeconds: int) =
                { state with
                      expireAfterSeconds = Some expireAfterSeconds }

            [<CustomOperation("hidden")>]
            member __.Hidden(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                             hidden: bool) =
                { state with hidden = Some hidden }

            [<CustomOperation("storage_engine")>]
            member __.StorageEngine(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                    storageEngine: 'StorageEngine) =
                { state with
                      storageEngine = Some storageEngine }

            [<CustomOperation("weights")>]
            member __.Weights(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                              weights: 'Weights) =
                { state with weights = Some weights }

            [<CustomOperation("default_language")>]
            member __.DefaultLanguage(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                      defaultLanguage: string) =
                { state with
                      default_language = Some defaultLanguage }

            [<CustomOperation("language_override")>]
            member __.LanguageOverride(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                       languageOverride: string) =
                { state with
                      language_override = Some languageOverride }

            [<CustomOperation("text_index_version")>]
            member __.TextIndexVersion(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                       textIndexVersion: int) =
                { state with
                      textIndexVersion = Some textIndexVersion }

            [<CustomOperation("2d_sphere_index_version")>]
            member __.``2dsphereIndexVersion``(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                               ``2dsphereIndexVersion``: int) =
                { state with
                      ``2dsphereIndexVersion`` = Some ``2dsphereIndexVersion`` }

            [<CustomOperation("bits")>]
            member __.Bits(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                           bits: int) =
                { state with bits = Some bits }

            [<CustomOperation("min")>]
            member __.Min(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                          min: float) =
                { state with min = Some min }

            [<CustomOperation("max")>]
            member __.Max(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                          max: float) =
                { state with max = Some max }

            [<CustomOperation("bucket_size")>]
            member __.BucketSize(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                 bucketSize: float) =
                { state with
                      bucketSize = Some bucketSize }

            [<CustomOperation("collation")>]
            member __.Collation(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                collation: Collation) =
                { state with
                      collation = Some collation }

            [<CustomOperation("wildcard_projection")>]
            member __.WildcardProjection(state: Index<'PartialFilterExpression, 'StorageEngine, 'Weights, 'WildcardProjection>,
                                         wildcardProjection: 'WildcardProjection) =
                { state with
                      wildcardProjection = Some wildcardProjection }

        type CreateIndexesBuilder(collection: string) =

            member __.Yield _ =
                { createIndexes = ""
                  indexes = Seq.empty
                  writeConcern = None
                  commitQuorum = None
                  comment = None }

            member __.Run(state: CreateIndexesCommand<'Index, 'WriteConcern, 'CommitQuorum, 'Comment>) =
                ({ state with
                       createIndexes = collection }
                :> IBuilder)
                    .ToJSON()

            [<CustomOperation("indexes")>]
            member __.Indexes(state: CreateIndexesCommand<'Index, 'WriteConcern, 'CommitQuorum, 'Comment>,
                              indexes: seq<'Index>) =
                { state with
                      indexes = indexes |> Seq.map box }

            [<CustomOperation("write_concern")>]
            member __.WriteConcern(state: CreateIndexesCommand<'Index, 'WriteConcern, 'CommitQuorum, 'Comment>,
                                   writeConcern: 'WriteConcern) =
                { state with
                      writeConcern = Some writeConcern }

            [<CustomOperation("commit_quorum")>]
            member __.CommitQuorum(state: CreateIndexesCommand<'Index, 'WriteConcern, 'CommitQuorum, 'Comment>,
                                   commitQuorum: 'CommitQuorum) =
                { state with
                      commitQuorum = Some commitQuorum }

            [<CustomOperation("comment")>]
            member __.Comment(state: CreateIndexesCommand<'Index, 'WriteConcern, 'CommitQuorum, 'Comment>,
                              comment: 'Comment) =
                { state with comment = Some comment }

        type DropIndexesBuilder(collection: string) =

            member __.Yield _ =
                { dropIndexes = ""
                  index = obj
                  writeConcern = None
                  comment = None }

            member __.Run(state: DropIndexCommand<'WriteConcern, 'Comment>) =
                ({ state with dropIndexes = collection } :> IBuilder)
                    .ToJSON()

            [<CustomOperation("index")>]
            member __.Index(state: DropIndexCommand<'WriteConcern, 'Comment>, index: obj) =
                { state with index = box index }

            member this.Index(state: DropIndexCommand<'WriteConcern, 'Comment>, index: string) =
                this.Index(state, index |> box)

            member this.Index(state: DropIndexCommand<'WriteConcern, 'Comment>, index: seq<obj>) =
                this.Index(state, index |> box)

            member this.Index(state: DropIndexCommand<'WriteConcern, 'Comment>, index: seq<string>) =
                this.Index(state, index |> box)

            [<CustomOperation("write_concern")>]
            member __.WriteConcern(state: DropIndexCommand<'WriteConcern, 'Comment>, writeConcern: 'WriteConcern) =
                { state with
                      writeConcern = Some writeConcern }

            [<CustomOperation("comment")>]
            member __.Comment(state: DropIndexCommand<'WriteConcern, 'Comment>, comment: 'Comment) =
                { state with comment = Some comment }

        let index (name: string) = IndexBuilder(name)

        let createIndexes (collection: string) = CreateIndexesBuilder(collection)

        let dropIndexes (collection: string) = DropIndexesBuilder(collection)
