using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    internal class GoTransaction<TContext> : IGoTransaction<TContext>
    {
        private readonly IGoContext<TContext> _goContext;
        private readonly IServiceProvider _serviceProvider;

        public GoTransaction(IGoContext<TContext> goContext,
                             IServiceProvider serviceProvider)
        {
            this._goContext = goContext;
            this._serviceProvider = serviceProvider;
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var session = _goContext.StartSession();
            session.StartTransaction();
            var collection = (IGoCollection<TDocument>) _serviceProvider.GetService(typeof(IGoCollection<TDocument>));

            throw new NotImplementedException();
        }

        public Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public GoDeleteResult DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<GoDeleteResult> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public GoDeleteResult DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<GoDeleteResult> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, GoFindOption<TDocument> goFindOption = null)
        {
            throw new NotImplementedException();
        }

        public void InsertMany<TDocument>(IEnumerable<TDocument> documents)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyAsync<TDocument>(IEnumerable<TDocument> documents)
        {
            throw new NotImplementedException();
        }

        public void InsertOne<TDocument>(TDocument document)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync<TDocument>(TDocument document)
        {
            throw new NotImplementedException();
        }

        public IGoBulker<TDocument> NewBulker<TDocument>()
        {
            throw new NotImplementedException();
        }

        public GoReplaceResult ReplaceOne<TDocument>(Expression<Func<TDocument, bool>> filter, TDocument document, bool isUpsert = false)
        {
            throw new NotImplementedException();
        }

        public Task<GoReplaceResult> ReplaceOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, TDocument document, bool isUpsert = false)
        {
            throw new NotImplementedException();
        }

        public GoUpdateResult UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            throw new NotImplementedException();
        }

        public Task<GoUpdateResult> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            throw new NotImplementedException();
        }

        public GoUpdateResult UpdateOne<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder, bool isUpsert = false)
        {
            throw new NotImplementedException();
        }

        public Task<GoUpdateResult> UpdateOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder, bool isUpsert = false)
        {
            throw new NotImplementedException();
        }
    }
}
