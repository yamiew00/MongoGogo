using System;

namespace MongoGogo.Connection
{
    public class GoFactory<TContext> : IGoFactory<TContext>
    {
        private readonly IServiceProvider _serviceProvider;

        public GoFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IGoBulker<TDocument> CreateBulker<TDocument>()
        {
            var collection = _serviceProvider.GetService(typeof(IGoCollection<TDocument>)) as IGoCollection<TDocument>;
            if (collection == null) throw new Exception($"The type '{typeof(TDocument).GetFriendlyName()}' must be decorated be MongoCollectionAttribute");
            return collection.NewBulker();
        }

        public IGoTransaction<TContext> CreateTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
