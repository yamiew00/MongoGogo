using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// MongoDb Context with type TContext.
    /// </summary>
    /// <typeparam name="TContext">The type of the mongodb context.</typeparam>
    public interface IGoContext<TContext> : IMongoClient
    {
        public IMongoDatabase GetDatabase(string name);
    }
}
