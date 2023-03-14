using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Concrete class of MongoDb Collection.
    /// </summary>
    /// <typeparam name="TDatabase"></typeparam>
    /// <typeparam name="TDocument"></typeparam>
    public class MongoCollection<TDatabase, TDocument> : IMongoCollection<TDocument>
    {
        private readonly IMongoCollection<TDocument> _MongoCollection;

        public MongoCollection(IGoDatabase<TDatabase> mongoDbDatabase)
        {
            //get collectionName using reflection. typeof(TDocument) must be decorated by MongoCollectionAttribute
            //todo: lifecycle of this class is Scope. Reflection may cause performance issue?
            var tMongoCollectionAttribute = typeof(TDocument).GetCustomAttribute<MongoCollectionAttribute>();
            var typeName = typeof(TDocument).Name;
            var collectionName = (tMongoCollectionAttribute == null) ? typeName : tMongoCollectionAttribute.GivenName ?? typeName;

            _MongoCollection = mongoDbDatabase.GetCollection<TDocument>(collectionName);
        }

        public CollectionNamespace CollectionNamespace => _MongoCollection.CollectionNamespace;

        public IMongoDatabase Database => _MongoCollection.Database;

        public IBsonSerializer<TDocument> DocumentSerializer => _MongoCollection.DocumentSerializer;

        public IMongoIndexManager<TDocument> Indexes => _MongoCollection.Indexes;

        public MongoCollectionSettings Settings => _MongoCollection.Settings;

        public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<TDocument, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Aggregate(pipeline, options, cancellationToken);
        }

        public IAsyncCursor<TResult> Aggregate<TResult>(IClientSessionHandle session,
                                                        PipelineDefinition<TDocument, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Aggregate(session, pipeline, options, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<TDocument, TResult> pipeline,
                                                                   AggregateOptions options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.AggregateAsync(pipeline, options, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(IClientSessionHandle session,
                                                                   PipelineDefinition<TDocument, TResult> pipeline,
                                                                   AggregateOptions options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.AggregateAsync(session, pipeline, options, cancellationToken);
        }

        public void AggregateToCollection<TResult>(PipelineDefinition<TDocument, TResult> pipeline,
                                                   AggregateOptions options = null,
                                                   CancellationToken cancellationToken = default)
        {
            _MongoCollection.AggregateToCollection(pipeline, options, cancellationToken);
        }

        public void AggregateToCollection<TResult>(IClientSessionHandle session,
                                                   PipelineDefinition<TDocument, TResult> pipeline,
                                                   AggregateOptions options = null,
                                                   CancellationToken cancellationToken = default)
        {
            _MongoCollection.AggregateToCollection(session, pipeline, options, cancellationToken);
        }

        public Task AggregateToCollectionAsync<TResult>(PipelineDefinition<TDocument, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoCollection.AggregateToCollectionAsync(pipeline, options, cancellationToken);
        }

        public Task AggregateToCollectionAsync<TResult>(IClientSessionHandle session,
                                                        PipelineDefinition<TDocument, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoCollection.AggregateToCollectionAsync(session, pipeline, options, cancellationToken);
        }

        public BulkWriteResult<TDocument> BulkWrite(IEnumerable<WriteModel<TDocument>> requests,
                                                    BulkWriteOptions options = null,
                                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.BulkWrite(requests, options, cancellationToken);
        }

        public BulkWriteResult<TDocument> BulkWrite(IClientSessionHandle session,
                                                    IEnumerable<WriteModel<TDocument>> requests,
                                                    BulkWriteOptions options = null,
                                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.BulkWrite(session, requests, options, cancellationToken);
        }

        public Task<BulkWriteResult<TDocument>> BulkWriteAsync(IEnumerable<WriteModel<TDocument>> requests,
                                                               BulkWriteOptions options = null,
                                                               CancellationToken cancellationToken = default)
        {
            return _MongoCollection.BulkWriteAsync(requests, options, cancellationToken);
        }

        public Task<BulkWriteResult<TDocument>> BulkWriteAsync(IClientSessionHandle session,
                                                               IEnumerable<WriteModel<TDocument>> requests,
                                                               BulkWriteOptions options = null,
                                                               CancellationToken cancellationToken = default)
        {
            return _MongoCollection.BulkWriteAsync(session, requests, options, cancellationToken);
        }

        public long Count(FilterDefinition<TDocument> filter,
                          CountOptions options = null,
                          CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Count(filter, options, cancellationToken);
        }

        public long Count(IClientSessionHandle session,
                          FilterDefinition<TDocument> filter,
                          CountOptions options = null,
                          CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Count(session, filter, options, cancellationToken);
        }

        public Task<long> CountAsync(FilterDefinition<TDocument> filter,
                                     CountOptions options = null,
                                     CancellationToken cancellationToken = default)
        {
            return _MongoCollection.CountAsync(filter, options, cancellationToken);
        }

        public Task<long> CountAsync(IClientSessionHandle session,
                                     FilterDefinition<TDocument> filter,
                                     CountOptions options = null,
                                     CancellationToken cancellationToken = default)
        {
            return _MongoCollection.CountAsync(session, filter, options, cancellationToken);
        }

        public long CountDocuments(FilterDefinition<TDocument> filter,
                                   CountOptions options = null,
                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.CountDocuments(filter, options, cancellationToken);
        }

        public long CountDocuments(IClientSessionHandle session,
                                   FilterDefinition<TDocument> filter,
                                   CountOptions options = null,
                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.CountDocuments(session, filter, options, cancellationToken);
        }

        public Task<long> CountDocumentsAsync(FilterDefinition<TDocument> filter,
                                              CountOptions options = null,
                                              CancellationToken cancellationToken = default)
        {
            return _MongoCollection.CountDocumentsAsync(filter, options, cancellationToken);
        }

        public Task<long> CountDocumentsAsync(IClientSessionHandle session,
                                              FilterDefinition<TDocument> filter,
                                              CountOptions options = null,
                                              CancellationToken cancellationToken = default)
        {
            return _MongoCollection.CountDocumentsAsync(session, filter, options, cancellationToken);
        }

        public DeleteResult DeleteMany(FilterDefinition<TDocument> filter,
                                       CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteMany(filter, cancellationToken);
        }

        public DeleteResult DeleteMany(FilterDefinition<TDocument> filter,
                                       DeleteOptions options,
                                       CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteMany(filter, options, cancellationToken);
        }

        public DeleteResult DeleteMany(IClientSessionHandle session,
                                       FilterDefinition<TDocument> filter,
                                       DeleteOptions options = null,
                                       CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteMany(session, filter, options, cancellationToken);
        }

        public Task<DeleteResult> DeleteManyAsync(FilterDefinition<TDocument> filter,
                                                  CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteManyAsync(filter, cancellationToken);
        }

        public Task<DeleteResult> DeleteManyAsync(FilterDefinition<TDocument> filter,
                                                  DeleteOptions options,
                                                  CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteManyAsync(filter, options, cancellationToken);
        }

        public Task<DeleteResult> DeleteManyAsync(IClientSessionHandle session,
                                                  FilterDefinition<TDocument> filter,
                                                  DeleteOptions options = null,
                                                  CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteManyAsync(session, filter, options, cancellationToken);
        }

        public DeleteResult DeleteOne(FilterDefinition<TDocument> filter,
                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteOne(filter, cancellationToken);
        }

        public DeleteResult DeleteOne(FilterDefinition<TDocument> filter,
                                      DeleteOptions options,
                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteOne(filter, options, cancellationToken);
        }

        public DeleteResult DeleteOne(IClientSessionHandle session,
                                      FilterDefinition<TDocument> filter,
                                      DeleteOptions options = null,
                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteOne(session, filter, options, cancellationToken);
        }

        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<TDocument> filter,
                                                 CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteOneAsync(filter, cancellationToken);
        }

        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<TDocument> filter,
                                                 DeleteOptions options,
                                                 CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteOneAsync(filter, options, cancellationToken);
        }

        public Task<DeleteResult> DeleteOneAsync(IClientSessionHandle session,
                                                 FilterDefinition<TDocument> filter,
                                                 DeleteOptions options = null,
                                                 CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DeleteOneAsync(session, filter, options, cancellationToken);
        }

        public IAsyncCursor<TField> Distinct<TField>(FieldDefinition<TDocument, TField> field,
                                                     FilterDefinition<TDocument> filter,
                                                     DistinctOptions options = null,
                                                     CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Distinct(field, filter, options, cancellationToken);
        }

        public IAsyncCursor<TField> Distinct<TField>(IClientSessionHandle session,
                                                     FieldDefinition<TDocument, TField> field,
                                                     FilterDefinition<TDocument> filter,
                                                     DistinctOptions options = null,
                                                     CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Distinct(session, field, filter, options, cancellationToken);
        }

        public Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldDefinition<TDocument, TField> field,
                                                                FilterDefinition<TDocument> filter,
                                                                DistinctOptions options = null,
                                                                CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DistinctAsync(field, filter, options, cancellationToken);
        }

        public Task<IAsyncCursor<TField>> DistinctAsync<TField>(IClientSessionHandle session,
                                                                FieldDefinition<TDocument, TField> field,
                                                                FilterDefinition<TDocument> filter,
                                                                DistinctOptions options = null,
                                                                CancellationToken cancellationToken = default)
        {
            return _MongoCollection.DistinctAsync(session, field, filter, options, cancellationToken);
        }

        public long EstimatedDocumentCount(EstimatedDocumentCountOptions options = null,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.EstimatedDocumentCount(options, cancellationToken);
        }

        public Task<long> EstimatedDocumentCountAsync(EstimatedDocumentCountOptions options = null,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.EstimatedDocumentCountAsync(options, cancellationToken);
        }

        public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(FilterDefinition<TDocument> filter,
                                                                      FindOptions<TDocument, TProjection> options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindAsync(filter, options, cancellationToken);
        }

        public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(IClientSessionHandle session,
                                                                      FilterDefinition<TDocument> filter,
                                                                      FindOptions<TDocument, TProjection> options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindAsync(session, filter, options, cancellationToken);
        }

        public TProjection FindOneAndDelete<TProjection>(FilterDefinition<TDocument> filter,
                                                         FindOneAndDeleteOptions<TDocument, TProjection> options = null,
                                                         CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndDelete(filter, options, cancellationToken);
        }

        public TProjection FindOneAndDelete<TProjection>(IClientSessionHandle session,
                                                         FilterDefinition<TDocument> filter,
                                                         FindOneAndDeleteOptions<TDocument, TProjection> options = null,
                                                         CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndDelete(session, filter, options, cancellationToken);
        }

        public Task<TProjection> FindOneAndDeleteAsync<TProjection>(FilterDefinition<TDocument> filter,
                                                                    FindOneAndDeleteOptions<TDocument, TProjection> options = null,
                                                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndDeleteAsync(filter, options, cancellationToken);
        }

        public Task<TProjection> FindOneAndDeleteAsync<TProjection>(IClientSessionHandle session,
                                                                    FilterDefinition<TDocument> filter,
                                                                    FindOneAndDeleteOptions<TDocument, TProjection> options = null,
                                                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndDeleteAsync(session, filter, options, cancellationToken);
        }

        public TProjection FindOneAndReplace<TProjection>(FilterDefinition<TDocument> filter,
                                                          TDocument replacement,
                                                          FindOneAndReplaceOptions<TDocument, TProjection> options = null,
                                                          CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndReplace(filter, replacement, options, cancellationToken);
        }

        public TProjection FindOneAndReplace<TProjection>(IClientSessionHandle session,
                                                          FilterDefinition<TDocument> filter,
                                                          TDocument replacement,
                                                          FindOneAndReplaceOptions<TDocument, TProjection> options = null,
                                                          CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndReplace(session, filter, replacement, options, cancellationToken);
        }

        public Task<TProjection> FindOneAndReplaceAsync<TProjection>(FilterDefinition<TDocument> filter,
                                                                     TDocument replacement,
                                                                     FindOneAndReplaceOptions<TDocument, TProjection> options = null,
                                                                     CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndReplaceAsync(filter, replacement, options, cancellationToken);
        }

        public Task<TProjection> FindOneAndReplaceAsync<TProjection>(IClientSessionHandle session,
                                                                     FilterDefinition<TDocument> filter,
                                                                     TDocument replacement,
                                                                     FindOneAndReplaceOptions<TDocument, TProjection> options = null,
                                                                     CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndReplaceAsync(session, filter, replacement, options, cancellationToken);
        }

        public TProjection FindOneAndUpdate<TProjection>(FilterDefinition<TDocument> filter,
                                                         UpdateDefinition<TDocument> update,
                                                         FindOneAndUpdateOptions<TDocument, TProjection> options = null,
                                                         CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndUpdate(filter, update, options, cancellationToken);
        }

        public TProjection FindOneAndUpdate<TProjection>(IClientSessionHandle session,
                                                         FilterDefinition<TDocument> filter,
                                                         UpdateDefinition<TDocument> update,
                                                         FindOneAndUpdateOptions<TDocument, TProjection> options = null,
                                                         CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndUpdate(session, filter, update, options, cancellationToken);
        }

        public Task<TProjection> FindOneAndUpdateAsync<TProjection>(FilterDefinition<TDocument> filter,
                                                                    UpdateDefinition<TDocument> update,
                                                                    FindOneAndUpdateOptions<TDocument, TProjection> options = null,
                                                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
        }

        public Task<TProjection> FindOneAndUpdateAsync<TProjection>(IClientSessionHandle session,
                                                                    FilterDefinition<TDocument> filter,
                                                                    UpdateDefinition<TDocument> update,
                                                                    FindOneAndUpdateOptions<TDocument, TProjection> options = null,
                                                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindOneAndUpdateAsync(session, filter, update, options, cancellationToken);
        }

        public IAsyncCursor<TProjection> FindSync<TProjection>(FilterDefinition<TDocument> filter,
                                                               FindOptions<TDocument, TProjection> options = null,
                                                               CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindSync(filter, options, cancellationToken);
        }

        public IAsyncCursor<TProjection> FindSync<TProjection>(IClientSessionHandle session,
                                                               FilterDefinition<TDocument> filter,
                                                               FindOptions<TDocument, TProjection> options = null,
                                                               CancellationToken cancellationToken = default)
        {
            return _MongoCollection.FindSync(session, filter, options, cancellationToken);
        }

        public void InsertMany(IEnumerable<TDocument> documents,
                               InsertManyOptions options = null,
                               CancellationToken cancellationToken = default)
        {
            _MongoCollection.InsertMany(documents, options, cancellationToken);
        }

        public void InsertMany(IClientSessionHandle session,
                               IEnumerable<TDocument> documents,
                               InsertManyOptions options = null,
                               CancellationToken cancellationToken = default)
        {
            _MongoCollection.InsertMany(session, documents, options, cancellationToken);
        }

        public Task InsertManyAsync(IEnumerable<TDocument> documents,
                                    InsertManyOptions options = null,
                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.InsertManyAsync(documents, options, cancellationToken);
        }

        public Task InsertManyAsync(IClientSessionHandle session,
                                    IEnumerable<TDocument> documents,
                                    InsertManyOptions options = null,
                                    CancellationToken cancellationToken = default)
        {
            return _MongoCollection.InsertManyAsync(session, documents, options, cancellationToken);
        }

        public void InsertOne(TDocument document,
                              InsertOneOptions options = null,
                              CancellationToken cancellationToken = default)
        {
            _MongoCollection.InsertOne(document, options, cancellationToken);
        }

        public void InsertOne(IClientSessionHandle session,
                              TDocument document,
                              InsertOneOptions options = null,
                              CancellationToken cancellationToken = default)
        {
            _MongoCollection.InsertOne(session, document, options, cancellationToken);
        }

        public Task InsertOneAsync(TDocument document,
                                   CancellationToken _cancellationToken)
        {
            return _MongoCollection.InsertOneAsync(document, _cancellationToken);
        }

        public Task InsertOneAsync(TDocument document,
                                   InsertOneOptions options = null,
                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.InsertOneAsync(document, options, cancellationToken);
        }

        public Task InsertOneAsync(IClientSessionHandle session,
                                   TDocument document,
                                   InsertOneOptions options = null,
                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.InsertOneAsync(session, document, options, cancellationToken);
        }

        public IAsyncCursor<TResult> MapReduce<TResult>(BsonJavaScript map,
                                                        BsonJavaScript reduce,
                                                        MapReduceOptions<TDocument, TResult> options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoCollection.MapReduce(map, reduce, options, cancellationToken);
        }

        public IAsyncCursor<TResult> MapReduce<TResult>(IClientSessionHandle session,
                                                        BsonJavaScript map,
                                                        BsonJavaScript reduce,
                                                        MapReduceOptions<TDocument, TResult> options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoCollection.MapReduce(session, map, reduce, options, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(BsonJavaScript map,
                                                                   BsonJavaScript reduce,
                                                                   MapReduceOptions<TDocument, TResult> options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.MapReduceAsync(map, reduce, options, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(IClientSessionHandle session,
                                                                   BsonJavaScript map,
                                                                   BsonJavaScript reduce,
                                                                   MapReduceOptions<TDocument, TResult> options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoCollection.MapReduceAsync(session, map, reduce, options, cancellationToken);
        }

        public IFilteredMongoCollection<TDerivedDocument> OfType<TDerivedDocument>() where TDerivedDocument : TDocument
        {
            return _MongoCollection.OfType<TDerivedDocument>();
        }

        public ReplaceOneResult ReplaceOne(FilterDefinition<TDocument> filter,
                                           TDocument replacement,
                                           ReplaceOptions options = null,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOne(filter, replacement, options, cancellationToken);
        }

        public ReplaceOneResult ReplaceOne(FilterDefinition<TDocument> filter,
                                           TDocument replacement,
                                           UpdateOptions options,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOne(filter, replacement, options, cancellationToken);
        }

        public ReplaceOneResult ReplaceOne(IClientSessionHandle session,
                                           FilterDefinition<TDocument> filter,
                                           TDocument replacement,
                                           ReplaceOptions options = null,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOne(session, filter, replacement, options, cancellationToken);
        }

        public ReplaceOneResult ReplaceOne(IClientSessionHandle session,
                                           FilterDefinition<TDocument> filter,
                                           TDocument replacement,
                                           UpdateOptions options,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOne(session, filter, replacement, options, cancellationToken);
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<TDocument> filter,
                                                      TDocument replacement,
                                                      ReplaceOptions options = null,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOneAsync(filter, replacement, options, cancellationToken);
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<TDocument> filter,
                                                      TDocument replacement,
                                                      UpdateOptions options,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOneAsync(filter, replacement, options, cancellationToken);
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session,
                                                      FilterDefinition<TDocument> filter,
                                                      TDocument replacement,
                                                      ReplaceOptions options = null,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOneAsync(session, filter, replacement, options, cancellationToken);
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session,
                                                      FilterDefinition<TDocument> filter,
                                                      TDocument replacement,
                                                      UpdateOptions options,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.ReplaceOneAsync(session, filter, replacement, options, cancellationToken);
        }

        public UpdateResult UpdateMany(FilterDefinition<TDocument> filter,
                                       UpdateDefinition<TDocument> update,
                                       UpdateOptions options = null,
                                       CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateMany(filter, update, options, cancellationToken);
        }

        public UpdateResult UpdateMany(IClientSessionHandle session,
                                       FilterDefinition<TDocument> filter,
                                       UpdateDefinition<TDocument> update,
                                       UpdateOptions options = null,
                                       CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateMany(session, filter, update, options, cancellationToken);
        }

        public Task<UpdateResult> UpdateManyAsync(FilterDefinition<TDocument> filter,
                                                  UpdateDefinition<TDocument> update,
                                                  UpdateOptions options = null,
                                                  CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateManyAsync(filter, update, options, cancellationToken);
        }

        public Task<UpdateResult> UpdateManyAsync(IClientSessionHandle session,
                                                  FilterDefinition<TDocument> filter,
                                                  UpdateDefinition<TDocument> update,
                                                  UpdateOptions options = null,
                                                  CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateManyAsync(session, filter, update, options, cancellationToken);
        }

        public UpdateResult UpdateOne(FilterDefinition<TDocument> filter,
                                      UpdateDefinition<TDocument> update,
                                      UpdateOptions options = null,
                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateOne(filter, update, options, cancellationToken);
        }

        public UpdateResult UpdateOne(IClientSessionHandle session,
                                      FilterDefinition<TDocument> filter,
                                      UpdateDefinition<TDocument> update,
                                      UpdateOptions options = null,
                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateOne(session, filter, update, options, cancellationToken);
        }

        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<TDocument> filter,
                                                 UpdateDefinition<TDocument> update,
                                                 UpdateOptions options = null,
                                                 CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateOneAsync(filter, update, options, cancellationToken);
        }

        public Task<UpdateResult> UpdateOneAsync(IClientSessionHandle session,
                                                 FilterDefinition<TDocument> filter,
                                                 UpdateDefinition<TDocument> update,
                                                 UpdateOptions options = null,
                                                 CancellationToken cancellationToken = default)
        {
            return _MongoCollection.UpdateOneAsync(session, filter, update, options, cancellationToken);
        }

        public IChangeStreamCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
                                                           ChangeStreamOptions options = null,
                                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Watch(pipeline, options, cancellationToken);
        }

        public IChangeStreamCursor<TResult> Watch<TResult>(IClientSessionHandle session,
                                                           PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
                                                           ChangeStreamOptions options = null,
                                                           CancellationToken cancellationToken = default)
        {
            return _MongoCollection.Watch(session, pipeline, options, cancellationToken);
        }

        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
                                                                      ChangeStreamOptions options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.WatchAsync(pipeline, options, cancellationToken);
        }

        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session,
                                                                      PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
                                                                      ChangeStreamOptions options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _MongoCollection.WatchAsync(session, pipeline, options, cancellationToken);
        }

        public IMongoCollection<TDocument> WithReadConcern(ReadConcern readConcern)
        {
            return _MongoCollection.WithReadConcern(readConcern);
        }

        public IMongoCollection<TDocument> WithReadPreference(ReadPreference readPreference)
        {
            return _MongoCollection.WithReadPreference(readPreference);
        }

        public IMongoCollection<TDocument> WithWriteConcern(WriteConcern writeConcern)
        {
            return _MongoCollection.WithWriteConcern(writeConcern);
        }
    }
}
