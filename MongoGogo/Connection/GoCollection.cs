
using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Default implementation of GoCollectionAbstract using DI.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    internal class GoCollection<TDocument> : GoCollectionAbstract<TDocument>
    {
        public GoCollection(IMongoCollection<TDocument> collection) : base(collection)
        {
        }
    }
}
