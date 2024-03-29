﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    internal class GoBulker<TDocument> : IGoBulker<TDocument>
    {
        protected List<WriteModel<TDocument>> _writeModels;

        protected readonly IMongoCollection<TDocument> _collection;

        internal GoBulker(IMongoCollection<TDocument> collection)
        {
            _writeModels = new List<WriteModel<TDocument>>();
            this._collection = collection;
        }

        public void InsertOne(TDocument document)
        {
            _writeModels.Add(new InsertOneModel<TDocument>(document));
        }

        public void InsertMany(IEnumerable<TDocument> documents)
        {
            foreach (var doc in documents)
            {
                _writeModels.Add(new InsertOneModel<TDocument>(doc));
            }
        }

        public void UpdateOne(Expression<Func<TDocument, bool>> filter,
                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                              bool isUpsert = false)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                           .Invoke(updateBuilder).MongoUpdateDefinition;

            _writeModels.Add(new UpdateOneModel<TDocument>(filter, mongoUpdateDefinition) { IsUpsert = isUpsert});
        }

        public void UpdateMany(Expression<Func<TDocument, bool>> filter,
                               Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                                               .Invoke(updateBuilder).MongoUpdateDefinition;
            _writeModels.Add(new UpdateManyModel<TDocument>(filter, mongoUpdateDefinition));
        }
        public void DeleteOne(Expression<Func<TDocument, bool>> filter)
        {
            _writeModels.Add(new DeleteOneModel<TDocument>(filter));
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filter)
        {
            _writeModels.Add(new DeleteManyModel<TDocument>(filter));
        }

        public void ReplaceOne(Expression<Func<TDocument, bool>> filter,
                               TDocument document,
                               bool isUpsert = false)
        {
            _writeModels.Add(new ReplaceOneModel<TDocument>(filter, document) { IsUpsert = isUpsert});
        }

        public virtual GoBulkResult SaveChanges()
        {
            if (!_writeModels.Any())
            {
                return new GoBulkResult(default);
            }

            var result = _collection.BulkWrite(_writeModels);
            _writeModels = new List<WriteModel<TDocument>>();
            return new GoBulkResult(result);
        }

        public virtual async Task<GoBulkResult> SaveChangesAsync()
        {
            if (!_writeModels.Any())
            {
                return new GoBulkResult(default);
            }

            var result = await _collection.BulkWriteAsync(_writeModels);
            _writeModels = new List<WriteModel<TDocument>>();
            return new GoBulkResult(result);
        }
    }
}
