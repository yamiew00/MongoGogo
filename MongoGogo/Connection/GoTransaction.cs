using MongoGogo.Connection.Builders.Deletes;
using MongoGogo.Connection.Builders.Replaces;
using MongoGogo.Connection.Builders.Updates;
using MongoGogo.Connection.Transactions;
using MongoGogo.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    internal class GoTransaction<TContext> : IGoTransaction<TContext>
    {
        private readonly IGoContext<TContext> _goContext;
        private readonly IServiceProvider _serviceProvider;

        private GoSession<TContext> _goSession { get; set; }

        public GoTransaction(GoTransactionOption option,
                             IGoContext<TContext> goContext,
                             IServiceProvider serviceProvider)
        {
            this._goContext = goContext;
            this._serviceProvider = serviceProvider;
            _goSession = new GoSession<TContext>(goContext, option);
        }

        private IGoCollection<TDocument> GetIGoCollection<TDocument>()
        {
            var result = _serviceProvider.GetService(typeof(IGoCollection<TDocument>)) as IGoCollection<TDocument>;
            if (result == null) throw new GoInvalidTypeException<TDocument>();

            return result;
        }

        public void Commit()
        {
            _goSession.CommitTransaction();
            this.Dispose();
        }

        public async Task CommitAsync()
        {
            await _goSession.CommitTransactionAsync();
            this.Dispose();
        }

        public void Dispose()
        {
            _goSession.Dispose();
        }

        public IGoTransBulker<TDocument> NewTransBulker<TDocument>()
        {
            return new GoTransBulker<TContext, TDocument>(GetIGoCollection<TDocument>().MongoCollection, this._goSession);
        }

        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.Count(_goSession.Session, filter);
        }

        public Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.CountAsync(_goSession.Session, filter);
        }

        public GoDeleteResult DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>(); 
            return collection.DeleteMany(_goSession.Session, filter);
        }

        public Task<GoDeleteResult> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.DeleteManyAsync(_goSession.Session, filter);
        }

        public GoDeleteResult DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.DeleteOne(_goSession.Session, filter);
        }

        public Task<GoDeleteResult> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.DeleteOneAsync(_goSession.Session, filter);
        }

        public TDocument DeleteOneAndRetrieve<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                         GoDeleteOneAndRetrieveOptions<TDocument> options = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.DeleteOneAndRetrieve(_goSession.Session, filter, options);
        }

        public Task<TDocument> DeleteOneAndRetrieveAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                    GoDeleteOneAndRetrieveOptions<TDocument> options = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.DeleteOneAndRetrieveAsync(_goSession.Session, filter, options);
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.Find(_goSession.Session, filter);
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                      Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                                      GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.Find(_goSession.Session, filter, projection, goFindOption);
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                      GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.Find(_goSession.Session, filter, default, goFindOption);
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindAsync(_goSession.Session, filter);
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                 Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                                                 GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindAsync(_goSession.Session, filter, projection, goFindOption);
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                 GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindAsync(_goSession.Session, filter, default, goFindOption);
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindOne(_goSession.Session, filter);
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                            Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                            GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindOne(_goSession.Session, filter, projection, goFindOption);
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                            GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindOne(_goSession.Session, filter, default, goFindOption);
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindOneAsync(_goSession.Session, filter);
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                                       GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindOneAsync(_goSession.Session, filter, projection, goFindOption);
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       GoFindOption<TDocument> goFindOption = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.FindOneAsync(_goSession.Session, filter, default, goFindOption);
        }

        public void InsertMany<TDocument>(IEnumerable<TDocument> documents)
        {
            var collection = GetIGoCollection<TDocument>();
            collection.InsertMany(_goSession.Session, documents);
        }

        public Task InsertManyAsync<TDocument>(IEnumerable<TDocument> documents)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.InsertManyAsync(_goSession.Session, documents);
        }

        public void InsertOne<TDocument>(TDocument document)
        {
            var collection = GetIGoCollection<TDocument>();
            collection.InsertOne(_goSession.Session, document);
        }

        public Task InsertOneAsync<TDocument>(TDocument document)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.InsertOneAsync(_goSession.Session, document);
        }

        public GoReplaceResult ReplaceOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                     TDocument document,
                                                     bool isUpsert = false)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.ReplaceOne(_goSession.Session, filter, document, isUpsert);
        }

        public Task<GoReplaceResult> ReplaceOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                TDocument document,
                                                                bool isUpsert = false)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.ReplaceOneAsync(_goSession.Session, filter, document, isUpsert);
        }

        public TDocument ReplaceOneAndRetrieve<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                          TDocument document,
                                                          GoReplaceOneAndRetrieveOptions<TDocument> options = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.ReplaceOneAndRetrieve(_goSession.Session, filter, document, options);
        }

        public Task<TDocument> ReplaceOneAndRetrieveAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                     TDocument document,
                                                                     GoReplaceOneAndRetrieveOptions<TDocument> options = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.ReplaceOneAndRetrieveAsync(_goSession.Session, filter, document, options);
        }

        public GoUpdateResult UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.UpdateMany(_goSession.Session, filter, updateDefinitionBuilder);
        }

        public Task<GoUpdateResult> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                               Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.UpdateManyAsync(_goSession.Session, filter, updateDefinitionBuilder);
        }

        public GoUpdateResult UpdateOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                   bool isUpsert = false)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.UpdateOne(_goSession.Session, filter, updateDefinitionBuilder, isUpsert);
        }

        public Task<GoUpdateResult> UpdateOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                              bool isUpsert = false)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.UpdateOneAsync(_goSession.Session, filter, updateDefinitionBuilder, isUpsert);
        }

        public TDocument UpdateOneAndRetrieve<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                         GoUpdateOneAndRetrieveOptions<TDocument> options = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.UpdateOneAndRetrieve(_goSession.Session, filter, updateDefinitionBuilder, options);
        }

        public Task<TDocument> UpdateOneAndRetrieveAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                                    Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                                                    GoUpdateOneAndRetrieveOptions<TDocument> options = null)
        {
            var collection = GetIGoCollection<TDocument>();
            return collection.UpdateOneAndRetrieveAsync(_goSession.Session, filter, updateDefinitionBuilder, projection, options);
        }
    }
}
