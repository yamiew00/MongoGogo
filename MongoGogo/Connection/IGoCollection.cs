using MongoDB.Driver;
using MongoGogo.Connection.Builders.Deletes;
using MongoGogo.Connection.Builders.Replaces;
using MongoGogo.Connection.Builders.Updates;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Represents a lightweight, user-friendly wrapper for the MongoDB collection object.
    /// </summary>
    /// <remarks>
    /// This interface provides an easy-to-use way to perform operations on MongoDB collections. It supports all the functionality provided by the official MongoDB driver's IMongoCollection interface. If necessary, you can still directly access the underlying IMongoCollection object for more advanced operations.
    /// </remarks>
    /// <typeparam name="TDocument"> The Document type</typeparam>
    public interface IGoCollection<TDocument>
    {
        /// <summary>
        /// Gets the underlying MongoDB collection object provided by the official MongoDB driver.
        /// </summary>
        public IMongoCollection<TDocument> MongoCollection { get; }

        /// <summary>
        /// Create new IGoBulker instance of this collection.
        /// </summary>
        public IGoBulker<TDocument> NewBulker();

        #region Basic operations

        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The returned documents.</returns>
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="projection">The projection</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>The returned documents.</returns>
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter,
                                           Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                           GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>The returned documents.</returns>
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter,
                                           GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Asynchronously finds the documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of documents that match the filter.</returns>
        public Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Asynchronously finds the documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="projection">The projection</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of documents that match the filter.</returns>
        public Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
                                                      Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                      GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Asynchronously finds the documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of documents that match the filter.</returns>
        public Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
                                                      GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Finds one document matching the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The returned document.</returns>
        public TDocument FindOne(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Finds one document matching the filter with a projection.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="projection">The projection</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>The returned document.</returns>
        public TDocument FindOne(Expression<Func<TDocument, bool>> filter,
                                 Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                 GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Finds one document matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>The returned document.</returns>
        public TDocument FindOne(Expression<Func<TDocument, bool>> filter,
                                 GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Asynchronously finds one document matching the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The returned document.</returns>
        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Asynchronously finds one document matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="projection">The projection</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>The returned document.</returns>
        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter,
                                            Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                            GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Asynchronously finds one document matching the filter.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="goFindOption">The option</param>
        /// <returns>The returned document.</returns>
        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter,
                                            GoFindOption<TDocument> goFindOption = default);

        /// <summary>
        /// Inserts a single document into the collection.
        /// </summary>
        /// <param name="document">The document to be inserted.</param>
        public void InsertOne(TDocument document);

        /// <summary>
        /// Asynchronously inserts a single document into the collection.
        /// </summary>
        /// <param name="document">The document to be inserted.</param>
        public Task InsertOneAsync(TDocument document);

        /// <summary>
        /// Inserts multiple documents into the collection.
        /// </summary>
        /// <param name="documents">The documents to be inserted.</param>
        public void InsertMany(IEnumerable<TDocument> documents);

        /// <summary>
        /// Asynchronously inserts multiple documents into the collection.
        /// </summary>
        /// <param name="documents">The documents to be inserted.</param>
        public Task InsertManyAsync(IEnumerable<TDocument> documents);

        /// <summary>
        /// Replaces a single document in the collection.
        /// </summary>
        /// <param name="filter">The filter to select the document.</param>
        /// <param name="document">The document to replace the existing one.</param>
        /// <param name="isUpsert">Whether to create a new document if no match is found.</param>
        /// <returns>Theresult of the replace operation.</returns>
        public GoReplaceResult ReplaceOne(Expression<Func<TDocument, bool>> filter,
                                          TDocument document,
                                          bool isUpsert = false);

        /// <summary>
        /// Asynchronously replaces a single document in the collection.
        /// </summary>
        /// <param name="filter">The filter to select the document.</param>
        /// <param name="document">The document to replace the existing one.</param>
        /// <param name="isUpsert">Whether to create a new document if no match is found.</param>
        /// <returns>The result of the replace operation.</returns>
        public Task<GoReplaceResult> ReplaceOneAsync(Expression<Func<TDocument, bool>> filter,
                                                     TDocument document,
                                                     bool isUpsert = false);

        /// <summary>
        /// Atomically replaces a single document matching the provided filter with the specified document and retrieves it.
        /// </summary>
        /// <param name="filter">The filter to select the document for replacement.</param>
        /// <param name="document">The new document to replace the existing one.</param>
        /// <param name="options">The options for the replace and retrieve operation, including whether to return the document pre-replace or post-replace and whether to upsert.</param>
        /// <returns>
        /// The document as it was before the replace or after the replace, based on the specified options.
        /// If no document matches the filter and 'IsUpsert' is false, returns null.
        /// When 'IsUpsert' is true, and 'ReturnDocument' is set to 'After', a new document is inserted if no match is found, and the inserted document is returned.
        /// This operation ensures that the document is retrieved in the same state as it was replaced.
        /// </returns>
        public TDocument ReplaceOneAndRetrieve(Expression<Func<TDocument, bool>> filter,
                                               TDocument document,
                                               GoReplaceOneAndRetrieveOptions<TDocument> options = default);

        /// <summary>
        /// Atomically replaces a single document matching the provided filter with the specified document and retrieves it asynchronously.
        /// </summary>
        /// <param name="filter">The filter to select the document for replacement.</param>
        /// <param name="document">The new document to replace the existing one.</param>
        /// <param name="options">The options for the replace and retrieve operation, including whether to return the document pre-replace or post-replace and whether to upsert.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is the document as it was before the replace or after the replace, based on the specified options.
        /// If no document matches the filter and 'IsUpsert' is false, the task result will be null.
        /// When 'IsUpsert' is true, and 'ReturnDocument' is set to 'After', a new document is inserted if no match is found, and the inserted document is returned as the task result.
        /// This operation ensures that the document is retrieved in the same state as it was replaced.
        /// </returns>
        public Task<TDocument> ReplaceOneAndRetrieveAsync(Expression<Func<TDocument, bool>> filter,
                                                          TDocument document,
                                                          GoReplaceOneAndRetrieveOptions<TDocument> options = default);


        /// <summary>
        /// Counts the number of documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The count of the documents.</returns>
        public long Count(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Asynchronously counts the number of documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The count of the documents.</returns>
        public Task<long> CountAsync(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Updates a single document matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the document.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply.</param>
        /// <param name="isUpsert">Whether to create a new document if no match is found.</param>
        /// <returns>The result of the update operation.</returns>
        public GoUpdateResult UpdateOne(Expression<Func<TDocument, bool>> filter,
                                        Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                        bool isUpsert = false);

        /// <summary>
        /// Asynchronously updates a single document matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the document.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply.</param>
        /// <param name="isUpsert">Whether to create a new document if no match is found.</param>
        /// <returns>The result of the update operation.</returns>
        public Task<GoUpdateResult> UpdateOneAsync(Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                   bool isUpsert = false);

        /// <summary>
        /// Atomically updates a single document matching the provided filter and retrieves it.
        /// </summary>
        /// <param name="filter">The filter to select the document for update.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply to the document.</param>
        /// <param name="options">The options for the update and retrieve operation, including whether to return the document pre-update or post-update and whether to upsert.</param>
        /// <returns>
        /// The document as it was before the update or after the update, based on the specified options.
        /// If no document matches the filter and 'IsUpsert' is false, returns null. 
        /// When 'IsUpsert' is true, and 'ReturnDocument' is set to 'After', a new document is inserted if no match is found, and the inserted document is returned.
        /// This operation ensures that the document is retrieved in the same state as it was updated.
        /// </returns>
        public TDocument UpdateOneAndRetrieve(Expression<Func<TDocument, bool>> filter,
                                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                              Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                              GoUpdateOneAndRetrieveOptions<TDocument> options = default);

        /// <summary>
        /// Atomically updates a single document matching the provided filter and retrieves it asynchronously.
        /// </summary>
        /// <param name="filter">The filter to select the document for update.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply to the document.</param>
        /// <param name="options">The options for the update and retrieve operation, including whether to return the document pre-update or post-update and whether to upsert.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is the document as it was before the update or after the update, based on the specified options.
        /// If no document matches the filter and 'IsUpsert' is false, the task result will be null.
        /// When 'IsUpsert' is true, and 'ReturnDocument' is set to 'After', a new document is inserted if no match is found, and the inserted document is returned as the task result.
        /// This operation ensures that the document is retrieved in the same state as it was updated.
        /// </returns>
        public Task<TDocument> UpdateOneAndRetrieveAsync(Expression<Func<TDocument, bool>> filter,
                                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                         Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                                         GoUpdateOneAndRetrieveOptions<TDocument> options = default);

        /// <summary>
        /// Updates multiple documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the documents.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply.</param>
        /// <returns>The result of the update operation.</returns>
        public GoUpdateResult UpdateMany(Expression<Func<TDocument, bool>> filter,
                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        /// <summary>
        /// Asynchronously updates multiple documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the documents.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply.</param>
        /// <returns>The result of the update operation.</returns>
        public Task<GoUpdateResult> UpdateManyAsync(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        /// <summary>
        /// Deletes a single document matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the document.</param>
        /// <returns>The result of the delete operation.</returns>
        public GoDeleteResult DeleteOne(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Asynchronously deletes a single document matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the document.</param>
        /// <returns>The result of the delete operation.</returns>
        public Task<GoDeleteResult> DeleteOneAsync(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Atomically deletes a single document matching the provided filter and retrieves it.
        /// </summary>
        /// <param name="filter">The filter to select the document for deletion.</param>
        /// <param name="options">The options for the delete and retrieve operation, including whether to return the document pre-deletion.</param>
        /// <returns>
        /// The document as it was before the deletion, based on the specified options.
        /// If no document matches the filter, returns null.
        /// This operation ensures that the document is retrieved in the same state as it was deleted.
        /// </returns>
        public TDocument DeleteOneAndRetrieve(Expression<Func<TDocument, bool>> filter,
                                              GoDeleteOneAndRetrieveOptions<TDocument> options = default);

        /// <summary>
        /// Atomically deletes a single document matching the provided filter and retrieves it asynchronously.
        /// </summary>
        /// <param name="filter">The filter to select the document for deletion.</param>
        /// <param name="options">The options for the delete and retrieve operation, including whether to return the document pre-deletion.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is the document as it was before the deletion, based on the specified options.
        /// If no document matches the filter, the task result will be null.
        /// This operation ensures that the document is retrieved in the same state as it was deleted.
        /// </returns>
        public Task<TDocument> DeleteOneAndRetrieveAsync(Expression<Func<TDocument, bool>> filter,
                                                         GoDeleteOneAndRetrieveOptions<TDocument> options = default);


        /// <summary>
        /// Deletes multiple documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the documents.</param>
        /// <returns>The result of the delete operation.</returns>
        public GoDeleteResult DeleteMany(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Asynchronously deletes multiple documents matching the filter.
        /// </summary>
        /// <param name="filter">The filter to select the documents.</param>
        /// <returns>The result of the delete operation.</returns>
        public Task<GoDeleteResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filter);

        #endregion

        #region Sessions

        internal IEnumerable<TDocument> Find(IClientSessionHandle session,
                                             Expression<Func<TDocument, bool>> filter,
                                             Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                             GoFindOption<TDocument> goFindOption = default);

        internal Task<IEnumerable<TDocument>> FindAsync(IClientSessionHandle session,
                                                        Expression<Func<TDocument, bool>> filter,
                                                        Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                        GoFindOption<TDocument> goFindOption = default);

        internal TDocument FindOne(IClientSessionHandle session,
                                   Expression<Func<TDocument, bool>> filter,
                                   Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                   GoFindOption<TDocument> goFindOption = default);

        internal Task<TDocument> FindOneAsync(IClientSessionHandle session,
                                              Expression<Func<TDocument, bool>> filter,
                                              Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                              GoFindOption<TDocument> goFindOption = default);

        internal void InsertOne(IClientSessionHandle session,
                                TDocument document);

        internal Task InsertOneAsync(IClientSessionHandle session,
                                     TDocument document);

        internal void InsertMany(IClientSessionHandle session,
                                 IEnumerable<TDocument> documents);

        internal Task InsertManyAsync(IClientSessionHandle session,
                                      IEnumerable<TDocument> documents);

        internal GoReplaceResult ReplaceOne(IClientSessionHandle session,
                                            Expression<Func<TDocument, bool>> filter,
                                            TDocument document,
                                            bool isUpsert = false);

        internal Task<GoReplaceResult> ReplaceOneAsync(IClientSessionHandle session,
                                                       Expression<Func<TDocument, bool>> filter,
                                                       TDocument document,
                                                       bool isUpsert = false);

        internal TDocument ReplaceOneAndRetrieve(IClientSessionHandle session,
                                                 Expression<Func<TDocument, bool>> filter,
                                                 TDocument document,
                                                 GoReplaceOneAndRetrieveOptions<TDocument> options);

        internal Task<TDocument> ReplaceOneAndRetrieveAsync(IClientSessionHandle session,
                                                            Expression<Func<TDocument, bool>> filter,
                                                            TDocument document,
                                                            GoReplaceOneAndRetrieveOptions<TDocument> options);

        internal long Count(IClientSessionHandle session,
                            Expression<Func<TDocument, bool>> filter);

        internal Task<long> CountAsync(IClientSessionHandle session,
                                       Expression<Func<TDocument, bool>> filter);

        internal GoUpdateResult UpdateOne(IClientSessionHandle session,
                                          Expression<Func<TDocument, bool>> filter,
                                          Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                          bool isUpsert = false);

        internal Task<GoUpdateResult> UpdateOneAsync(IClientSessionHandle session,
                                                     Expression<Func<TDocument, bool>> filter,
                                                     Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                     bool isUpsert = false);

        internal TDocument UpdateOneAndRetrieve(IClientSessionHandle session,
                                                Expression<Func<TDocument, bool>> filter,
                                                Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                GoUpdateOneAndRetrieveOptions<TDocument> options = default);

        internal Task<TDocument> UpdateOneAndRetrieveAsync(IClientSessionHandle session,
                                                           Expression<Func<TDocument, bool>> filter,
                                                           Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                           Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                                           GoUpdateOneAndRetrieveOptions<TDocument> options = default);

        internal GoUpdateResult UpdateMany(IClientSessionHandle session,
                                           Expression<Func<TDocument, bool>> filter,
                                           Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        internal Task<GoUpdateResult> UpdateManyAsync(IClientSessionHandle session,
                                                      Expression<Func<TDocument, bool>> filter,
                                                      Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        internal GoDeleteResult DeleteOne(IClientSessionHandle session,
                                          Expression<Func<TDocument, bool>> filter);

        internal Task<GoDeleteResult> DeleteOneAsync(IClientSessionHandle session,
                                                     Expression<Func<TDocument, bool>> filter);

        internal TDocument DeleteOneAndRetrieve(IClientSessionHandle session,
                                                Expression<Func<TDocument, bool>> filter,
                                                GoDeleteOneAndRetrieveOptions<TDocument> options = default);

        internal Task<TDocument> DeleteOneAndRetrieveAsync(IClientSessionHandle session,
                                                           Expression<Func<TDocument, bool>> filter,
                                                           GoDeleteOneAndRetrieveOptions<TDocument> options = default);

        internal GoDeleteResult DeleteMany(IClientSessionHandle session,
                                           Expression<Func<TDocument, bool>> filter);

        internal Task<GoDeleteResult> DeleteManyAsync(IClientSessionHandle session,
                                                      Expression<Func<TDocument, bool>> filter);
        #endregion
    }
}
