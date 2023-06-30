using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// An object responsible for performing Bulk Operations.
    /// </summary>
    /// <typeparam name="TDocument">mongo document</typeparam>
    public interface IGoBulker<TDocument>
    {
        /// <summary>
        /// Schedules an operation to insert a single document into the MongoDB collection. 
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="document">the document to be inserted</param>
        public void InsertOne(TDocument document);

        /// <summary>
        /// Schedules an operation to insert multiple documents into the MongoDB collection. 
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="documents"></param>
        public void InsertMany(IEnumerable<TDocument> documents);

        /// <summary>
        /// Schedules an operation to update a single document in the MongoDB collection based on a filter.
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="filter">The filter to select the document to update.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply.</param>
        /// <param name="isUpsert">Whether to perform an upsert operation.</param>
        public void UpdateOne(Expression<Func<TDocument, bool>> filter,
                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                              bool isUpsert = false);

        /// <summary>
        /// Schedules an operation to update multiple documents in the MongoDB collection based on a filter.
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="filter">The filter to select the documents to update.</param>
        /// <param name="updateDefinitionBuilder">The builder to create the update operation to apply.</param>
        public void UpdateMany(Expression<Func<TDocument, bool>> filter,
                               Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder);

        /// <summary>
        /// Schedules an operation to replace a single document in the MongoDB collection based on a filter.
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="filter">The filter to select the document to replace.</param>
        /// <param name="document">The new document.</param>
        /// <param name="isUpsert">Whether to perform an upsert operation.</param>
        public void ReplaceOne(Expression<Func<TDocument, bool>> filter,
                               TDocument document,
                               bool isUpsert = false);

        /// <summary>
        /// Schedules an operation to delete a single document from the MongoDB collection based on a filter.
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="filter">The filter to select the document to delete.</param>
        public void DeleteOne(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Schedules an operation to delete multiple documents from the MongoDB collection based on a filter.
        /// The operation is not executed until SaveChanges or SaveChangesAsync is called.
        /// </summary>
        /// <param name="filter">The filter to select the documents to delete.</param>
        public void DeleteMany(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Executes all scheduled operations in a bulk write to the MongoDB collection. 
        /// </summary>
        /// <returns>A result object with details of the executed operations.</returns>
        public GoBulkResult SaveChanges();

        /// <summary>
        /// Executes all scheduled operations in a bulk write to the MongoDB collection asynchronously. 
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a result object with details of the executed operations.</returns>
        public Task<GoBulkResult> SaveChangesAsync();
    }
}
