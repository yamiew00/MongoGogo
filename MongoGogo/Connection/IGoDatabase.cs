using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Database interface.
    /// Same to IMongoDatabase in Mongo Driver. Main difference is that IMongoDatabase are not in generic type.
    /// </summary>
    /// <typeparam name="TDatabase">databaseType</typeparam>
    public interface IGoDatabase<TDatabase> : IMongoDatabase
    {
    }
}
