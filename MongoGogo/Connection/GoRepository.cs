
using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Default implementation of GoRepositoryAbstract using DI.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    internal class GoRepository<TDocument> : GoRepositoryAbstract<TDocument>
    {
        public GoRepository(IMongoCollection<TDocument> collection) : base(collection)
        {
        }
    }
}
