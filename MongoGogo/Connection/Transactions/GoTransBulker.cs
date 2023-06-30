using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoGogo.Connection.Transactions
{
    internal class GoTransBulker<TContext, TDocument> : GoBulker<TDocument>, IGoTransBulker<TDocument>
    {
        private readonly GoSession<TContext> _goSession;

        internal GoTransBulker(IMongoCollection<TDocument> collection,
                               GoSession<TContext> goSession) : base(collection)
        {
            this._goSession = goSession;
        }

        public override GoBulkResult SaveChanges()
        {
            var result = _collection.BulkWrite(_goSession.Session, _writeModels);
            _writeModels = new List<WriteModel<TDocument>>();
            return new GoBulkResult(result);
        }

        public override async Task<GoBulkResult> SaveChangesAsync()
        {
            var result = await _collection.BulkWriteAsync(_goSession.Session, _writeModels);
            _writeModels = new List<WriteModel<TDocument>>();
            return new GoBulkResult(result);
        }
    }
}
