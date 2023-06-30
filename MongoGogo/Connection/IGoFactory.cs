using MongoGogo.Connection.Transactions;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Factory for creating transaction and bulker.
    /// </summary>
    /// <typeparam name="TContext">The type of the mongodb context</typeparam>
    public interface IGoFactory<TContext>
    {
        /// <summary>
        /// Gets an IGoTransaction for performing transactional operations.
        /// </summary>
        /// <returns>The IGoTransaction</returns>
        /// <remarks>Calling this method does not initiate a session or transaction.</remarks>
        public IGoTransaction<TContext> CreateTransaction(GoTransactionOption option = default);
        
        /// <summary>
        /// Gets an IGoBulker for performing bulk write operations on a single collection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns>The IGoBulker.</returns>
        public IGoBulker<TDocument> CreateBulker<TDocument>();
    }
}
