using MongoDB.Driver;

namespace MongoGogo.Connection
{
    public interface IMongoContext<TContext>
    {
        public IMongoDatabase GetDatabase(string name);
    }
}
