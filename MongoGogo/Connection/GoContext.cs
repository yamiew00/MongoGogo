using MongoDB.Driver;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Abstract class of a IMongoContext.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class GoContext<TContext> : IGoContext<TContext>
    {
        private readonly IMongoClient _Client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">connectionString of a MongoDb connection.</param>
        public GoContext(string connectionString)
        {
            _Client = new MongoClient(connectionString);
        }

        /// <summary>
        /// the way MongoClient get its database.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(string name)
        {
            return _Client.GetDatabase(name);
        }
    }
}
