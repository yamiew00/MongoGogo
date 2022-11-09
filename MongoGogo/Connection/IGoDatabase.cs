using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// MongoDB Database interface.
    /// Same to IMongoDatabase in Mongo Driver but in generic type.
    /// </summary>
    /// <typeparam name="TDatabase">databaseType</typeparam>
    public interface IGoDatabase<TDatabase> : IMongoDatabase
    {
    }
}
