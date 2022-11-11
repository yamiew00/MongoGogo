using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    public interface IGoRepository<TDocument>
    {
        public IMongoCollection<TDocument> MongoCollection { get; }

        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter);

        public Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter);

        public TDocument FindOne(Expression<Func<TDocument, bool>> filter);

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter);

        public void InsertOne(TDocument document);

        public Task InsertOneAsync(TDocument document);

        public void InsertMany(IEnumerable<TDocument> documents);

        public Task InsertManyAsync(IEnumerable<TDocument> documents);

        public void ReplaceOne(Expression<Func<TDocument, bool>> filter, TDocument document, ReplaceOptions replaceOptions = default);

        public Task ReplaceOneAsync(Expression<Func<TDocument, bool>> filter, TDocument document, ReplaceOptions replaceOptions = default);

        public long Count(Expression<Func<TDocument, bool>> filter);

        public Task<long> CountAsync(Expression<Func<TDocument, bool>> filter);
    }
}
