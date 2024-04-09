using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using System.Threading;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Abstract class of a IMongoContext.
    /// </summary>
    /// <typeparam name="TContext">The type of the mongodb context.</typeparam>
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

        public GoContext(MongoClientSettings clientSettings)
        {
            _Client = new MongoClient(clientSettings);
        }

        public ICluster Cluster => _Client.Cluster;

        public MongoClientSettings Settings => _Client.Settings;

        public void DropDatabase(string name, CancellationToken cancellationToken = default)
        {
            _Client.DropDatabase(name, cancellationToken);
        }

        public void DropDatabase(IClientSessionHandle session, string name, CancellationToken cancellationToken = default)
        {
            _Client.DropDatabase(session, name, cancellationToken);
        }

        public Task DropDatabaseAsync(string name, CancellationToken cancellationToken = default)
        {
            return _Client.DropDatabaseAsync(name, cancellationToken);
        }

        public Task DropDatabaseAsync(IClientSessionHandle session, string name, CancellationToken cancellationToken = default)
        {
            return _Client.DropDatabaseAsync(session, name, cancellationToken);
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

        public IMongoDatabase GetDatabase(string name, MongoDatabaseSettings settings = null)
        {
            return _Client.GetDatabase(name, settings);
        }

        public IAsyncCursor<string> ListDatabaseNames(CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNames(cancellationToken);
        }

        public IAsyncCursor<string> ListDatabaseNames(ListDatabaseNamesOptions options, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNames(options, cancellationToken);
        }

        public IAsyncCursor<string> ListDatabaseNames(IClientSessionHandle session, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNames(session, cancellationToken);
        }

        public IAsyncCursor<string> ListDatabaseNames(IClientSessionHandle session, ListDatabaseNamesOptions options, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNames(session, options, cancellationToken);
        }

        public Task<IAsyncCursor<string>> ListDatabaseNamesAsync(CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNamesAsync(cancellationToken);
        }

        public Task<IAsyncCursor<string>> ListDatabaseNamesAsync(ListDatabaseNamesOptions options,
                                                                 CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNamesAsync(options, cancellationToken);
        }

        public Task<IAsyncCursor<string>> ListDatabaseNamesAsync(IClientSessionHandle session, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNamesAsync(session, cancellationToken);
        }

        public Task<IAsyncCursor<string>> ListDatabaseNamesAsync(IClientSessionHandle session,
                                                                 ListDatabaseNamesOptions options,
                                                                 CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabaseNamesAsync(session, options, cancellationToken);
        }

        public IAsyncCursor<BsonDocument> ListDatabases(CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabases(cancellationToken);
        }

        public IAsyncCursor<BsonDocument> ListDatabases(ListDatabasesOptions options, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabases(options, cancellationToken);
        }

        public IAsyncCursor<BsonDocument> ListDatabases(IClientSessionHandle session, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabases(session, cancellationToken);
        }

        public IAsyncCursor<BsonDocument> ListDatabases(IClientSessionHandle session, ListDatabasesOptions options, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabases(session, options, cancellationToken);
        }

        public Task<IAsyncCursor<BsonDocument>> ListDatabasesAsync(CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabasesAsync(cancellationToken);
        }

        public Task<IAsyncCursor<BsonDocument>> ListDatabasesAsync(ListDatabasesOptions options, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabasesAsync(options, cancellationToken);
        }

        public Task<IAsyncCursor<BsonDocument>> ListDatabasesAsync(IClientSessionHandle session, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabasesAsync(session, cancellationToken);
        }

        public Task<IAsyncCursor<BsonDocument>> ListDatabasesAsync(IClientSessionHandle session, ListDatabasesOptions options, CancellationToken cancellationToken = default)
        {
            return _Client.ListDatabasesAsync(session, options, cancellationToken);
        }

        public IClientSessionHandle StartSession(ClientSessionOptions options = null, CancellationToken cancellationToken = default)
        {
            return _Client.StartSession(options, cancellationToken);
        }

        public Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default)
        {
            return _Client.StartSessionAsync(options, cancellationToken);
        }

        public IChangeStreamCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                           ChangeStreamOptions options = null,
                                                           CancellationToken cancellationToken = default)
        {
            return _Client.Watch(pipeline, options, cancellationToken);
        }

        public IChangeStreamCursor<TResult> Watch<TResult>(IClientSessionHandle session,
                                                           PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                           ChangeStreamOptions options = null,
                                                           CancellationToken cancellationToken = default)
        {
            return _Client.Watch(session, pipeline, options, cancellationToken);
        }

        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                                      ChangeStreamOptions options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _Client.WatchAsync(pipeline, options, cancellationToken);
        }

        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session,
                                                                      PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                                      ChangeStreamOptions options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _Client.WatchAsync(session, pipeline, options, cancellationToken);
        }

        public IMongoClient WithReadConcern(ReadConcern readConcern)
        {
            return _Client.WithReadConcern(readConcern);
        }

        public IMongoClient WithReadPreference(ReadPreference readPreference)
        {
            return _Client.WithReadPreference(readPreference);
        }

        public IMongoClient WithWriteConcern(WriteConcern writeConcern)
        {
            return _Client.WithWriteConcern(writeConcern);
        }
    }
}
