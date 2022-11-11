using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace MongoGogo.Connection
{
    /// <summary>
    /// An abstract mongo repository.
    /// </summary>
    /// <typeparam name="TDocument">mongo document</typeparam>
    public abstract class GoRepositoryAbstract<TDocument> : IGoRepository<TDocument>
    {
        /// <summary>
        /// the IMongoCollection<TDocuement> instance.
        /// </summary>
        public IMongoCollection<TDocument> MongoCollection { get; private set; }

        public GoRepositoryAbstract(IGoCollection<TDocument> collection)
        {
            MongoCollection = collection;
        }

        public virtual long Count(Expression<Func<TDocument, bool>> filter)
        {
            return MongoCollection.CountDocuments(filter);
        }

        public virtual async Task<long> CountAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await MongoCollection.CountDocumentsAsync(filter);
        }

        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter)
        {
            return MongoCollection.Find(filter).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter)
        {
            return (await MongoCollection.FindAsync(filter)).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filter)
        {
            return MongoCollection.Find(filter).Limit(1).FirstOrDefault();
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            return (await MongoCollection.FindAsync(filter)).FirstOrDefault();
        }

        public virtual void InsertMany(IEnumerable<TDocument> documents)
        {
            MongoCollection.InsertMany(documents);
        }

        public virtual Task InsertManyAsync(IEnumerable<TDocument> documents)
        {
            return MongoCollection.InsertManyAsync(documents);
        }

        public virtual void InsertOne(TDocument document)
        {
            MongoCollection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return MongoCollection.InsertOneAsync(document);
        }

        public virtual void ReplaceOne(Expression<Func<TDocument, bool>> filter, TDocument document, ReplaceOptions replaceOptions = default)
        {
            MongoCollection.ReplaceOne(filter, document, replaceOptions);
        }

        public virtual Task ReplaceOneAsync(Expression<Func<TDocument, bool>> filter, TDocument document, ReplaceOptions replaceOptions = default)
        {
            return MongoCollection.ReplaceOneAsync(filter, document, replaceOptions);
        }

        public GoUpdateResult UpdateOne(Expression<Func<TDocument, bool>> filter,
                                        Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> set,
                                        bool isUpsert = false)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = set.Compile()
                                           .Invoke(updateBuilder).MongoUpdateDefinition;
            var mongoUpdateResult = MongoCollection.UpdateOne(filter, 
                                          mongoUpdateDefinition, 
                                          new UpdateOptions
                                          {
                                              IsUpsert = isUpsert
                                          });
            return new GoUpdateResult(mongoUpdateResult);
        }

        public async Task<GoUpdateResult> UpdateOneAsync(Expression<Func<TDocument, bool>> filter,
                                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> set,
                                                         bool isUpsert = false)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = set.Compile()
                                           .Invoke(updateBuilder).MongoUpdateDefinition;
            var mongoUpdateResult = await MongoCollection.UpdateOneAsync(filter,
                                                                         mongoUpdateDefinition,
                                                                         new UpdateOptions
                                                                         {
                                                                             IsUpsert = isUpsert
                                                                         });
            return new GoUpdateResult(mongoUpdateResult);
        }

        public GoUpdateResult UpdateMany(Expression<Func<TDocument, bool>> filter,
                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> set)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = set.Compile()
                                           .Invoke(updateBuilder).MongoUpdateDefinition;
            var mongoUpdateResult = MongoCollection.UpdateMany(filter,
                                                               mongoUpdateDefinition);
            return new GoUpdateResult(mongoUpdateResult);
        }

        public async Task<GoUpdateResult> UpdateManyAsync(Expression<Func<TDocument, bool>> filter,
                                                          Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> set)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = set.Compile()
                                           .Invoke(updateBuilder).MongoUpdateDefinition;
            var mongoUpdateResult = await MongoCollection.UpdateManyAsync(filter,
                                                                          mongoUpdateDefinition);
            return new GoUpdateResult(mongoUpdateResult);
        }
    }
}
