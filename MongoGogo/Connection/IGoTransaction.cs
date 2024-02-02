using MongoGogo.Connection.Builders.Deletes;
using MongoGogo.Connection.Builders.Replaces;
using MongoGogo.Connection.Builders.Updates;
using MongoGogo.Connection.Transactions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// A MongoDB transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the MongoDB context.</typeparam>
    public interface IGoTransaction<TContext> : IDisposable
    {
        /// <summary>
        /// Commits the MongoDB transaction synchronously.
        /// </summary>
        /// <remarks>
        /// After committing, the transaction becomes unusable.
        /// </remarks>
        public void Commit();

        /// <summary>
        /// Commits the MongoDB transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// After committing, the transaction becomes unusable.
        /// </remarks>
        public Task CommitAsync();

        /// <summary>
        /// Creates a new bulker instance for performing bulk write operations within the transaction.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns>An instance of IGoTransBulker allowing bulk write operations.</returns>
        /// <remarks>
        /// Any bulk write operations performed will only take effect after the transaction is committed.
        /// </remarks>
        public IGoTransBulker<TDocument> NewTransBulker<TDocument>();

        #region Basic Operations
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

        public TDocument ReplaceOneAndRetrieve<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                          TDocument document,
                                                          GoReplaceOneAndRetrieveOptions<TDocument> options = default);

        public Task<TDocument> ReplaceOneAndRetrieveAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                     TDocument document,
                                                                     GoReplaceOneAndRetrieveOptions<TDocument> options = default);

        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        public GoUpdateResult UpdateOne<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                   bool isUpsert = false);

        public Task<GoUpdateResult> UpdateOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                              bool isUpsert = false);

        public TDocument UpdateOneAndRetrieve<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                 Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                 GoUpdateOneAndRetrieveOptions<TDocument> options = default);

        public Task<TDocument> UpdateOneAndRetrieveAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                                    GoUpdateOneAndRetrieveOptions<TDocument> options = default);

        public GoUpdateResult UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        public Task<GoUpdateResult> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                               Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        public GoDeleteResult DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<GoDeleteResult> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        public TDocument DeleteOneAndRetrieve<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                         GoDeleteOneAndRetrieveOptions<TDocument> options = default);

        public Task<TDocument> DeleteOneAndRetrieveAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                                    GoDeleteOneAndRetrieveOptions<TDocument> options = default);

        public GoDeleteResult DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter);

        public Task<GoDeleteResult> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

        #endregion
    }
}
