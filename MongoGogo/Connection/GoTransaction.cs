using MongoDB.Driver;
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
        private readonly GoTransactionOption _option;
        private readonly IGoContext<TContext> _goContext;
        private readonly IServiceProvider _serviceProvider;

        private IClientSessionHandle _session;

        private GoTransactionStatus _status { get; set; }

        public GoTransaction(GoTransactionOption option,
                             IGoContext<TContext> goContext,
                             IServiceProvider serviceProvider)
        {
            this._option = option;
            this._goContext = goContext;
            this._serviceProvider = serviceProvider;
            _status = new GoTransactionStatus();
        }

        private IGoCollection<TDocument> TryStartAndGetGoCollection<TDocument>()
        {
            if (!_status.IsSessionStart)
            {
                var sessionOption = new ClientSessionOptions();
                if (_option != null && 
                   _option.CausalConsistency.HasValue && 
                   !_option.CausalConsistency.Value)
                {
                    sessionOption.CausalConsistency = false;
                }

                _session = _goContext.StartSession(sessionOption);
                _session.StartTransaction();
                _status.IsSessionStart = true;
            }
            _status.HasAnyOperation = true;
            var result = (IGoCollection<TDocument>)_serviceProvider.GetService(typeof(IGoCollection<TDocument>));
            if (result == null) throw new GoInvalidTypeException<TDocument>();
            return result;
        }

        public void Commit()
        {
            if(_status.HasAnyOperation) _session.CommitTransaction();
            this.Dispose();
        }

        public async Task CommitAsync()
        {
            if (_status.HasAnyOperation) await _session.CommitTransactionAsync();
            this.Dispose();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }

        public IGoBulker<TDocument> NewBulker<TDocument>()
        {
            throw new NotImplementedException();
        }

        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.Count(_session, filter);
        }

        public Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.CountAsync(_session, filter);
        }

        public GoDeleteResult DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>(); 
            return collection.DeleteMany(_session, filter);
        }

        public Task<GoDeleteResult> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.DeleteManyAsync(_session, filter);
        }

        public GoDeleteResult DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.DeleteOne(_session, filter);
        }

        public Task<GoDeleteResult> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.DeleteOneAsync(_session, filter);
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.Find(_session, filter);
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.Find(_session, filter, projection, goFindOption);
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.Find(_session, filter, default, goFindOption);
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindAsync(_session, filter);
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindAsync(_session, filter, projection, goFindOption);
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindAsync(_session, filter, default, goFindOption);
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindOne(_session, filter);
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindOne(_session, filter, projection, goFindOption);
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindOne(_session, filter, default, goFindOption);
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindOneAsync(_session, filter);
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindOneAsync(_session, filter, projection, goFindOption);
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.FindOneAsync(_session, filter, default, goFindOption);
        }

        public void InsertMany<TDocument>(IEnumerable<TDocument> documents)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            collection.InsertMany(_session, documents);
        }

        public Task InsertManyAsync<TDocument>(IEnumerable<TDocument> documents)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.InsertManyAsync(_session, documents);
        }

        public void InsertOne<TDocument>(TDocument document)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            collection.InsertOne(_session, document);
        }

        public Task InsertOneAsync<TDocument>(TDocument document)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.InsertOneAsync(_session, document);
        }

        public GoReplaceResult ReplaceOne<TDocument>(Expression<Func<TDocument, bool>> filter, TDocument document, bool isUpsert = false)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.ReplaceOne(_session, filter, document, isUpsert);
        }

        public Task<GoReplaceResult> ReplaceOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, TDocument document, bool isUpsert = false)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.ReplaceOneAsync(_session, filter, document, isUpsert);
        }

        public GoUpdateResult UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.UpdateMany(_session, filter, updateDefinitionBuilder);
        }

        public Task<GoUpdateResult> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.UpdateManyAsync(_session, filter, updateDefinitionBuilder);
        }

        public GoUpdateResult UpdateOne<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder, bool isUpsert = false)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.UpdateOne(_session, filter, updateDefinitionBuilder, isUpsert);
        }

        public Task<GoUpdateResult> UpdateOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder, bool isUpsert = false)
        {
            var collection = TryStartAndGetGoCollection<TDocument>();
            return collection.UpdateOneAsync(_session, filter, updateDefinitionBuilder, isUpsert);
        }
    }
}
