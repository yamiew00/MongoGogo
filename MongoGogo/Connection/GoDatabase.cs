using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// A MongoDb Database.
    /// </summary>
    /// <typeparam name="TContext">The type of the mongodb context.</typeparam>
    /// <typeparam name="TDatabase">database type.</typeparam>
    internal class GoDatabase<TContext, TDatabase> : IGoDatabase<TDatabase>
    {
        private readonly IMongoDatabase _MongoDatabase; 

        public GoDatabase(IGoContext<TContext> mongoContext)
        {
            //get databaseName using reflection. typeof(TMongoDatabase) must be decorated by MongoDatabaseAttribute
            var tMongoDatabaseAttribute = typeof(TDatabase).GetCustomAttribute<MongoDatabaseAttribute>();
            var typeName = typeof(TDatabase).Name;
            var databaseName = (tMongoDatabaseAttribute == null) ? typeName : tMongoDatabaseAttribute.GivenName ?? typeName;

            _MongoDatabase = mongoContext.GetDatabase(databaseName);
        }

        public IMongoClient Client => _MongoDatabase.Client;

        public DatabaseNamespace DatabaseNamespace => _MongoDatabase.DatabaseNamespace;

        public MongoDatabaseSettings Settings => _MongoDatabase.Settings;

        public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.Aggregate(pipeline, options, cancellationToken);
        }

        public IAsyncCursor<TResult> Aggregate<TResult>(IClientSessionHandle session,
                                                        PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.Aggregate(session, pipeline, options, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                                   AggregateOptions options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.AggregateAsync(pipeline, options, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(IClientSessionHandle session,
                                                                   PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                                   AggregateOptions options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.AggregateAsync(session, pipeline, options, cancellationToken);
        }

        public void AggregateToCollection<TResult>(PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                   AggregateOptions options = null,
                                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.AggregateToCollection(pipeline, options, cancellationToken);
        }

        public void AggregateToCollection<TResult>(IClientSessionHandle session,
                                                   PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                   AggregateOptions options = null,
                                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.AggregateToCollection(session, pipeline, options, cancellationToken);
        }

        public Task AggregateToCollectionAsync<TResult>(PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.AggregateToCollectionAsync(pipeline, options, cancellationToken);
        }

        public Task AggregateToCollectionAsync<TResult>(IClientSessionHandle session,
                                                        PipelineDefinition<NoPipelineInput, TResult> pipeline,
                                                        AggregateOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.AggregateToCollectionAsync(session, pipeline, options, cancellationToken);
        }

        public void CreateCollection(string name,
                                     CreateCollectionOptions options = null,
                                     CancellationToken cancellationToken = default)
        {
            _MongoDatabase.CreateCollection(name, options, cancellationToken);
        }

        public void CreateCollection(IClientSessionHandle session,
                                     string name,
                                     CreateCollectionOptions options = null,
                                     CancellationToken cancellationToken = default)
        {
            _MongoDatabase.CreateCollection(session, name, options, cancellationToken);
        }

        public Task CreateCollectionAsync(string name,
                                          CreateCollectionOptions options = null,
                                          CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.CreateCollectionAsync(name, options, cancellationToken);
        }

        public Task CreateCollectionAsync(IClientSessionHandle session,
                                          string name,
                                          CreateCollectionOptions options = null,
                                          CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.CreateCollectionAsync(session, name, options, cancellationToken);
        }

        public void CreateView<TDocument, TResult>(string viewName,
                                                   string viewOn,
                                                   PipelineDefinition<TDocument, TResult> pipeline,
                                                   CreateViewOptions<TDocument> options = null,
                                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.CreateView(viewName, viewOn, pipeline, options, cancellationToken);
        }

        public void CreateView<TDocument, TResult>(IClientSessionHandle session,
                                                   string viewName,
                                                   string viewOn,
                                                   PipelineDefinition<TDocument, TResult> pipeline,
                                                   CreateViewOptions<TDocument> options = null,
                                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.CreateView(session, viewName, viewOn, pipeline, options, cancellationToken);
        }

        public Task CreateViewAsync<TDocument, TResult>(string viewName,
                                                        string viewOn,
                                                        PipelineDefinition<TDocument, TResult> pipeline,
                                                        CreateViewOptions<TDocument> options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.CreateViewAsync(viewName, viewOn, pipeline, options, cancellationToken);
        }

        public Task CreateViewAsync<TDocument, TResult>(IClientSessionHandle session,
                                                        string viewName,
                                                        string viewOn,
                                                        PipelineDefinition<TDocument, TResult> pipeline,
                                                        CreateViewOptions<TDocument> options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.CreateViewAsync(session, viewName, viewOn, pipeline, options, cancellationToken);
        }

        public void DropCollection(string name,
                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.DropCollection(name, cancellationToken);
        }

        public void DropCollection(string name,
                                   DropCollectionOptions options,
                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.DropCollection(name, options, cancellationToken);
        }

        public void DropCollection(IClientSessionHandle session,
                                   string name,
                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.DropCollection(session, name, cancellationToken);
        }

        public void DropCollection(IClientSessionHandle session,
                                   string name,
                                   DropCollectionOptions options,
                                   CancellationToken cancellationToken = default)
        {
            _MongoDatabase.DropCollection(session, name, options, cancellationToken);
        }

        public Task DropCollectionAsync(string name,
                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.DropCollectionAsync(name, cancellationToken);
        }

        public Task DropCollectionAsync(string name,
                                        DropCollectionOptions options,
                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.DropCollectionAsync(name, options, cancellationToken);
        }

        public Task DropCollectionAsync(IClientSessionHandle session,
                                        string name,
                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.DropCollectionAsync(session, name, cancellationToken);
        }

        public Task DropCollectionAsync(IClientSessionHandle session,
                                        string name,
                                        DropCollectionOptions options,
                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.DropCollectionAsync(session, name, options, cancellationToken);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name,
                                                                    MongoCollectionSettings settings = null)
        {
            return _MongoDatabase.GetCollection<TDocument>(name, settings);
        }

        public IAsyncCursor<string> ListCollectionNames(ListCollectionNamesOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollectionNames(options, cancellationToken);
        }

        public IAsyncCursor<string> ListCollectionNames(IClientSessionHandle session,
                                                        ListCollectionNamesOptions options = null,
                                                        CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollectionNames(session, options, cancellationToken);
        }

        public Task<IAsyncCursor<string>> ListCollectionNamesAsync(ListCollectionNamesOptions options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollectionNamesAsync(options, cancellationToken);
        }

        public Task<IAsyncCursor<string>> ListCollectionNamesAsync(IClientSessionHandle session,
                                                                   ListCollectionNamesOptions options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollectionNamesAsync(session, options, cancellationToken);
        }

        public IAsyncCursor<BsonDocument> ListCollections(ListCollectionsOptions options = null,
                                                          CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollections(options, cancellationToken);
        }

        public IAsyncCursor<BsonDocument> ListCollections(IClientSessionHandle session,
                                                          ListCollectionsOptions options = null,
                                                          CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollections(session, options, cancellationToken);
        }

        public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null,
                                                                     CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollectionsAsync(options, cancellationToken);
        }

        public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(IClientSessionHandle session,
                                                                     ListCollectionsOptions options = null,
                                                                     CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.ListCollectionsAsync(session, options, cancellationToken);
        }

        public void RenameCollection(string oldName,
                                     string newName,
                                     RenameCollectionOptions options = null,
                                     CancellationToken cancellationToken = default)
        {
            _MongoDatabase.RenameCollection(oldName, newName, options, cancellationToken);
        }

        public void RenameCollection(IClientSessionHandle session,
                                     string oldName,
                                     string newName,
                                     RenameCollectionOptions options = null,
                                     CancellationToken cancellationToken = default)
        {
            _MongoDatabase.RenameCollection(session, oldName, newName, options, cancellationToken);
        }

        public Task RenameCollectionAsync(string oldName,
                                          string newName,
                                          RenameCollectionOptions options = null,
                                          CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.RenameCollectionAsync(oldName, newName, options, cancellationToken);
        }

        public Task RenameCollectionAsync(IClientSessionHandle session,
                                          string oldName,
                                          string newName,
                                          RenameCollectionOptions options = null,
                                          CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.RenameCollectionAsync(session, oldName, newName, options, cancellationToken);
        }

        public TResult RunCommand<TResult>(Command<TResult> command,
                                           ReadPreference readPreference = null,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.RunCommand(command, readPreference, cancellationToken);
        }

        public TResult RunCommand<TResult>(IClientSessionHandle session,
                                           Command<TResult> command,
                                           ReadPreference readPreference = null,
                                           CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.RunCommand(session, command, readPreference, cancellationToken);
        }

        public Task<TResult> RunCommandAsync<TResult>(Command<TResult> command,
                                                      ReadPreference readPreference = null,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.RunCommandAsync(command, readPreference, cancellationToken);
        }

        public Task<TResult> RunCommandAsync<TResult>(IClientSessionHandle session,
                                                      Command<TResult> command,
                                                      ReadPreference readPreference = null,
                                                      CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.RunCommandAsync(session, command, readPreference, cancellationToken);
        }

        public IChangeStreamCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                           ChangeStreamOptions options = null,
                                                           CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.Watch(pipeline, options, cancellationToken);
        }

        public IChangeStreamCursor<TResult> Watch<TResult>(IClientSessionHandle session,
                                                           PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                           ChangeStreamOptions options = null,
                                                           CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.Watch(session, pipeline, options, cancellationToken);
        }

        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                                      ChangeStreamOptions options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.WatchAsync(pipeline, options, cancellationToken);
        }

        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session,
                                                                      PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline,
                                                                      ChangeStreamOptions options = null,

                                                                      CancellationToken cancellationToken = default)
        {
            return _MongoDatabase.WatchAsync(session, pipeline, options, cancellationToken);
        }

        public IMongoDatabase WithReadConcern(ReadConcern readConcern)
        {
            return _MongoDatabase.WithReadConcern(readConcern);
        }

        public IMongoDatabase WithReadPreference(ReadPreference readPreference)
        {
            return _MongoDatabase.WithReadPreference(readPreference);
        }

        public IMongoDatabase WithWriteConcern(WriteConcern writeConcern)
        {
            return _MongoDatabase.WithWriteConcern(writeConcern);
        }
    }
}
