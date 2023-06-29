using MongoGogo.Connection.Transactions;
using System;

namespace MongoGogo.Connection
{
    internal class GoFactory<TContext> : IGoFactory<TContext>
    {
        private readonly IGoContext<TContext> _goContext;
        private readonly IServiceProvider _serviceProvider;

        public GoFactory(IGoContext<TContext> goContext,
                         IServiceProvider serviceProvider)
        {
            this._goContext = goContext;
            this._serviceProvider = serviceProvider;
        }

        public IGoBulker<TDocument> CreateBulker<TDocument>()
        {
            var collection = _serviceProvider.GetService(typeof(IGoCollection<TDocument>)) as IGoCollection<TDocument>;
            if (collection == null) throw new Exception($"The type '{typeof(TDocument).GetFriendlyName()}' must be decorated by MongoCollectionAttribute");
            return collection.NewBulker();
        }

        public IGoTransaction<TContext> CreateTransaction(GoTransactionOption option = null)
        {
            return new GoTransaction<TContext>(option, _goContext, _serviceProvider);
        }
    }
}
