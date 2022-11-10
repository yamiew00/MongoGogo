using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// MongoDb Context with type TContext.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IGoContext<TContext> : IMongoClient
    {
        public IMongoDatabase GetDatabase(string name);
    }
}
