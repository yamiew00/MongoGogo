using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Represents the result of a bulk operation.
    /// </summary>
    public class GoBulkResult
    {
        /// <summary>
        /// Gets the number of documents that were deleted.
        /// </summary>
        public long DeletedCount { get; }

        /// <summary>
        /// Gets the number of documents that were inserted.
        /// </summary>
        public long InsertedCount { get; }

        /// <summary>
        /// Gets a value indicating whether the bulk write operation was acknowledged.
        /// </summary>
        public bool IsAcknowledged { get; }

        /// <summary>
        /// Gets a value indicating whether the modified count is available.
        /// </summary>
        /// <remarks>
        /// The available modified count.
        /// </remarks>
        public bool IsModifiedCountAvailable { get; }

        /// <summary>
        /// Gets the number of documents that were matched.
        /// </summary>
        public long MatchedCount { get; }

        /// <summary>
        /// Gets the number of documents that were actually modified during an update.
        /// </summary>
        public long ModifiedCount { get; }

        /// <summary>
        /// Gets the request count.
        /// </summary>
        public int RequestCount { get; }

        internal GoBulkResult(BulkWriteResult bulkWriteResult)
        {
            DeletedCount = bulkWriteResult.DeletedCount;
            InsertedCount = bulkWriteResult.InsertedCount;
            IsAcknowledged = bulkWriteResult.IsAcknowledged;
            ModifiedCount = bulkWriteResult.ModifiedCount;
            RequestCount = bulkWriteResult.RequestCount;
            MatchedCount = bulkWriteResult.MatchedCount;
            ModifiedCount = bulkWriteResult.ModifiedCount;
            IsModifiedCountAvailable = bulkWriteResult.IsModifiedCountAvailable;
        }
    }
}
