using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext">The type of the mongodb context.</typeparam>
    public interface IGoTransaction<TContext> : IDisposable
    {
        public void Commit();

        public Task CommitAsync();

        #region CRUD
        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter);

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                      Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                      GoFindOption<TDocument> goFindOption = default);

        public IEnumerable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                      GoFindOption<TDocument> goFindOption = default);

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                 Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                                 GoFindOption<TDocument> goFindOption = default);

        public Task<IEnumerable<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                 GoFindOption<TDocument> goFindOption = default);

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter);

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                            Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                            GoFindOption<TDocument> goFindOption = default);

        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                            GoFindOption<TDocument> goFindOption = default);

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                       GoFindOption<TDocument> goFindOption = default);

        public Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       GoFindOption<TDocument> goFindOption = default);

        public void InsertOne<TDocument>(TDocument document);

        public Task InsertOneAsync<TDocument>(TDocument document);

        public void InsertMany<TDocument>(IEnumerable<TDocument> documents);

        public Task InsertManyAsync<TDocument>(IEnumerable<TDocument> documents);

        public GoReplaceResult ReplaceOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                     TDocument document,
                                                     bool isUpsert = false);

        public Task<GoReplaceResult> ReplaceOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                TDocument document,
                                                                bool isUpsert = false);

        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        public GoUpdateResult UpdateOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                   bool isUpsert = false);

        public Task<GoUpdateResult> UpdateOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                              bool isUpsert = false);

        public GoUpdateResult UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        public Task<GoUpdateResult> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                               Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        public GoDeleteResult DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<GoDeleteResult> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        public GoDeleteResult DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<GoDeleteResult> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        #endregion

        public IGoBulker<TDocument> NewBulker<TDocument>();
    }

}
