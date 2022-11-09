using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Collection interface.
    /// Same to IMongoCollection<TDocument> in Mongo Driver
    /// </summary>
    /// <typeparam name="TDocument">Document in MongoDb</typeparam>
    public interface IGoCollection<TDocument> : IMongoCollection<TDocument>
    {
    }
}
