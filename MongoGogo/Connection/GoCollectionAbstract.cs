﻿using MongoDB.Driver;
using MongoGogo.Connection.Builders.Finds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace MongoGogo.Connection
{
    /// <summary>
    /// An abstract mongo collection.
    /// </summary>
    /// <typeparam name="TDocument">mongo document</typeparam>
    public abstract class GoCollectionAbstract<TDocument> : IGoCollection<TDocument>
    {
        /// <summary>
        /// the IMongoCollection<TDocuement> instance.
        /// </summary>
        public IMongoCollection<TDocument> MongoCollection { get; private set; }

        public GoCollectionAbstract(IMongoCollection<TDocument> collection)
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

        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,    
                                                   GoFindOption<TDocument> goFindOption = default)
        {
            goFindOption ??= new GoFindOption<TDocument>();
            var builder = new GoProjectionBuilder<TDocument>();

            var findFluent = MongoCollection.Find(filter, new FindOptions
            {
                AllowDiskUse = goFindOption.AllowDiskUse
            });

            if (projection != null)
            {
                findFluent = findFluent.Project<TDocument>(projection?.Compile().Invoke(builder).MongoProjectionDefinition);
            }

            if(goFindOption.Sort != default)
            {
                findFluent = SortedFluent(findFluent, goFindOption.Sort);
            }

            return findFluent.Limit(goFindOption.Limit)
                                  .Skip(goFindOption.Skip)
                                  .ToEnumerable();
        }

        private IFindFluent<TDocument, TDocument> SortedFluent(IFindFluent<TDocument, TDocument> findFluent,
                                                               Expression<Func<GoSortBuilder<TDocument>, GoSortDefinition<TDocument>>> sort)
        {
            var goSortBuilder = new GoSortBuilder<TDocument>();
            var goSortDefinition = sort.Compile().Invoke(goSortBuilder);

            //deal primary
            IOrderedFindFluent<TDocument, TDocument> orderedFluent;
            var primarySortRule = goSortDefinition._primarySortRule;
            if (primarySortRule.OrderType == OrderType.Ascending)
            {
                orderedFluent = findFluent.SortBy(primarySortRule.KeySelector);
            }
            else if (primarySortRule.OrderType == OrderType.Descending)
            {
                orderedFluent = findFluent.SortByDescending(primarySortRule.KeySelector);
            }
            else throw new NotImplementedException();

            //deal secondary
            foreach (var secondarySortRule in goSortDefinition._secondarySortRules)
            {
                if(secondarySortRule.OrderType == OrderType.Ascending)
                {
                    orderedFluent = orderedFluent.ThenBy(secondarySortRule.KeySelector);
                }
                else if(secondarySortRule.OrderType == OrderType.Descending)
                {
                    orderedFluent = orderedFluent.ThenByDescending(secondarySortRule.KeySelector);
                }
            }

            return orderedFluent;
        }

        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter,
                                                   GoFindOption<TDocument> goFindOption = default,
                                                   Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default)
        {
            return Find(filter, projection, goFindOption);
        }

        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await (await MongoCollection.FindAsync(filter)).ToListAsync();
        }

        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
                                                                    Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                                    GoFindOption<TDocument> goFindOption = default)
        {
            goFindOption ??= new GoFindOption<TDocument>();
            var builder = new GoProjectionBuilder<TDocument>();
            var findOptions = new FindOptions<TDocument, TDocument>
            {
                AllowDiskUse = goFindOption.AllowDiskUse,
                Limit = goFindOption.Limit,
                Skip = goFindOption.Skip,
                Sort = ToSort(goFindOption)
            };

            if (projection != null)
            {
                findOptions.Projection = projection?.Compile().Invoke(builder).MongoProjectionDefinition;
            }

            return await (await MongoCollection.FindAsync(filter, findOptions)).ToListAsync();
        }

        private SortDefinition<TDocument> ToSort(GoFindOption<TDocument> goFindOption)
        {
            if (goFindOption.Sort == null) return default;
            var goSortBuilder = new GoSortBuilder<TDocument>();
            var goSortDefinition = goFindOption.Sort.Compile().Invoke(goSortBuilder);

            //deal primary
            SortDefinition<TDocument> sortDefinition = default;
            var primarySortRule = goSortDefinition._primarySortRule;
            if(primarySortRule.OrderType == OrderType.Ascending)
            {
                sortDefinition = Builders<TDocument>.Sort.Ascending(primarySortRule.KeySelector);
            }
            else if( primarySortRule.OrderType == OrderType.Descending)
            {
                sortDefinition = Builders<TDocument>.Sort.Descending(primarySortRule.KeySelector);
            }
            else throw new NotImplementedException();

            //deal secondary
            foreach (var secondarySortRule in goSortDefinition._secondarySortRules)
            {
                if (secondarySortRule.OrderType == OrderType.Ascending)
                {
                    sortDefinition = sortDefinition.Ascending(secondarySortRule.KeySelector);
                }
                else if (secondarySortRule.OrderType == OrderType.Descending)
                {
                    sortDefinition = sortDefinition.Descending(secondarySortRule.KeySelector);
                }
                else throw new NotImplementedException();
            }

            return sortDefinition;
        }

        public Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
                                                      GoFindOption<TDocument> goFindOption = null,
                                                      Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null)
        {
            return FindAsync(filter, projection, goFindOption);
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filter)
        {
            return MongoCollection.Find(filter).Limit(1).FirstOrDefault();
        }


        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filter,
                                         Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                         GoFindOption<TDocument> goFindOption = null)
        {
            goFindOption ??= new GoFindOption<TDocument>();
            goFindOption.Limit = 1;

            return Find(filter, projection, goFindOption).FirstOrDefault();
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filter,
                                 GoFindOption<TDocument> goFindOption = null,
                                 Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null)
        {
            return FindOne(filter, projection, goFindOption);
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            return (await MongoCollection.FindAsync(filter)).FirstOrDefault();
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter,
                                                          Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                                          GoFindOption<TDocument> goFindOption = null)
        {
            if(goFindOption == null)
            {
                goFindOption = new GoFindOption<TDocument>
                {
                    Limit = 1
                };
            }
            else
            {
                goFindOption.Limit = 1;
            }

            return (await FindAsync(filter, projection, goFindOption)).FirstOrDefault();
        }

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter,
                                            GoFindOption<TDocument> goFindOption = null,
                                            Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null)
        {
            return FindOneAsync(filter, projection, goFindOption);
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

        public virtual GoReplaceResult ReplaceOne(Expression<Func<TDocument, bool>> filter, TDocument document, bool isUpsert = false)
        {
            var replaceResult = MongoCollection.ReplaceOne(filter, document, new ReplaceOptions
            {
                IsUpsert = isUpsert
            });
            return new GoReplaceResult(replaceResult);
        }

        public virtual async Task<GoReplaceResult> ReplaceOneAsync(Expression<Func<TDocument, bool>> filter,
                                                                   TDocument document,
                                                                   bool isUpsert = false)
        {
            var replaceResult = await MongoCollection.ReplaceOneAsync(filter, document, new ReplaceOptions
            {
                IsUpsert = isUpsert
            });
            return new GoReplaceResult(replaceResult);
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

        public GoDeleteResult DeleteOne(Expression<Func<TDocument, bool>> filter)
        {
            return new GoDeleteResult(MongoCollection.DeleteOne(filter));
        }

        public async Task<GoDeleteResult> DeleteOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            return new GoDeleteResult( await MongoCollection.DeleteOneAsync(filter));
        }

        public GoDeleteResult DeleteMany(Expression<Func<TDocument, bool>> filter)
        {
            return new GoDeleteResult(MongoCollection.DeleteMany(filter));
        }

        public async Task<GoDeleteResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filter)
        {
            return new GoDeleteResult(await MongoCollection.DeleteManyAsync(filter));
        }

        public IGoBulker<TDocument> NewBulker()
        {
            return new GoBulker<TDocument>(MongoCollection);
        }
    }
}
