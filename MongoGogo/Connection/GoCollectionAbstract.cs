using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoGogo.Connection.Builders.Deletes;
using MongoGogo.Connection.Builders.Finds;
using MongoGogo.Connection.Builders.Replaces;
using MongoGogo.Connection.Builders.Updates;
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
            return PrimaryMethodCaller().Count(default, filter);
        }

        public virtual Task<long> CountAsync(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().CountAsync(default, filter);
        }

        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().Find(default, filter, default, default);
        }

        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                   GoFindOption<TDocument> goFindOption = default)
        {
            return PrimaryMethodCaller().Find(default, filter, projection, goFindOption);
        }

        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter,
                                                   GoFindOption<TDocument> goFindOption = default)
        {
            return PrimaryMethodCaller().Find(default, filter, default, goFindOption);
        }

        public virtual Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().FindAsync(default, filter, default, default);
        }

        public virtual Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
                                                              Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = default,
                                                              GoFindOption<TDocument> goFindOption = default)
        {
            return PrimaryMethodCaller().FindAsync(default, filter, projection, goFindOption);
        }

        public virtual Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
                                                              GoFindOption<TDocument> goFindOption = null)
        {
            return PrimaryMethodCaller().FindAsync(default, filter, default, goFindOption);
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().FindOne(default, filter, default, default);
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filter,
                                         Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                         GoFindOption<TDocument> goFindOption = null)
        {
            return PrimaryMethodCaller().FindOne(default, filter, projection, goFindOption);
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filter,
                                 GoFindOption<TDocument> goFindOption = null)
        {
            return PrimaryMethodCaller().FindOne(default, filter, default, goFindOption);
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().FindOneAsync(default, filter, default, default);
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection = null,
                                                    GoFindOption<TDocument> goFindOption = null)
        {
            return PrimaryMethodCaller().FindOneAsync(default, filter, projection, goFindOption);
        }

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter,
                                            GoFindOption<TDocument> goFindOption = null)
        {
            return PrimaryMethodCaller().FindOneAsync(default, filter, default, goFindOption);
        }

        public virtual void InsertMany(IEnumerable<TDocument> documents)
        {
            PrimaryMethodCaller().InsertMany(default, documents);
        }

        public virtual Task InsertManyAsync(IEnumerable<TDocument> documents)
        {
            return PrimaryMethodCaller().InsertManyAsync(default, documents);
        }

        public virtual void InsertOne(TDocument document)
        {
            PrimaryMethodCaller().InsertOne(default, document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return PrimaryMethodCaller().InsertOneAsync(default, document);
        }

        public virtual GoReplaceResult ReplaceOne(Expression<Func<TDocument, bool>> filter,
                                                  TDocument document,
                                                  bool isUpsert = false)
        {
            return PrimaryMethodCaller().ReplaceOne(default, filter, document, isUpsert);  
        }

        public virtual Task<GoReplaceResult> ReplaceOneAsync(Expression<Func<TDocument, bool>> filter,
                                                             TDocument document,
                                                             bool isUpsert = false)
        {
            return PrimaryMethodCaller().ReplaceOneAsync(default, filter, document, isUpsert);
        }

        public TDocument ReplaceOneAndRetrieve(Expression<Func<TDocument, bool>> filter,
                                               TDocument document,
                                               GoReplaceOneAndRetrieveOptions<TDocument> options)
        {
            return PrimaryMethodCaller().ReplaceOneAndRetrieve(default, filter, document, options);
        }

        public Task<TDocument> ReplaceOneAndRetrieveAsync(Expression<Func<TDocument, bool>> filter,
                                                          TDocument document,
                                                          GoReplaceOneAndRetrieveOptions<TDocument> options)
        {
            return PrimaryMethodCaller().ReplaceOneAndRetrieveAsync(default, filter, document, options);
        }

        public GoUpdateResult UpdateOne(Expression<Func<TDocument, bool>> filter,
                                        Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                        bool isUpsert = false)
        {
            return PrimaryMethodCaller().UpdateOne(default, filter, updateDefinitionBuilder, isUpsert);
        }

        public  Task<GoUpdateResult> UpdateOneAsync(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                    bool isUpsert = false)
        {
            return PrimaryMethodCaller().UpdateOneAsync(default, filter, updateDefinitionBuilder, isUpsert);
        }

        public TDocument UpdateOneAndRetrieve(Expression<Func<TDocument, bool>> filter,
                                              Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                              GoUpdateOneAndRetrieveOptions<TDocument> options = default)
        {
            return PrimaryMethodCaller().UpdateOneAndRetrieve(default, filter, updateDefinitionBuilder, options);
        }

        public Task<TDocument> UpdateOneAndRetrieveAsync(Expression<Func<TDocument, bool>> filter,
                                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                         GoUpdateOneAndRetrieveOptions<TDocument> options = default)
        {
            return PrimaryMethodCaller().UpdateOneAndRetrieveAsync(default, filter, updateDefinitionBuilder, options);
        }

        public GoUpdateResult UpdateMany(Expression<Func<TDocument, bool>> filter,
                                         Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            return PrimaryMethodCaller().UpdateMany(default, filter, updateDefinitionBuilder);
        }

        public Task<GoUpdateResult> UpdateManyAsync(Expression<Func<TDocument, bool>> filter,
                                                    Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            return PrimaryMethodCaller().UpdateManyAsync(default, filter, updateDefinitionBuilder);
        }

        public GoDeleteResult DeleteOne(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().DeleteOne(default, filter);
        }

        public Task<GoDeleteResult> DeleteOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().DeleteOneAsync(default, filter);
        }


        public TDocument DeleteOneAndRetrieve(Expression<Func<TDocument, bool>> filter,
                                              GoDeleteOneAndRetrieveOptions<TDocument> options = null)
        {
            return PrimaryMethodCaller().DeleteOneAndRetrieve(default, filter, options);
        }

        public Task<TDocument> DeleteOneAndRetrieveAsync(Expression<Func<TDocument, bool>> filter,
                                                         GoDeleteOneAndRetrieveOptions<TDocument> options = null)
        {
            return PrimaryMethodCaller().DeleteOneAndRetrieveAsync(default, filter, options);
        }

        public GoDeleteResult DeleteMany(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().DeleteMany(default, filter);
        }

        public Task<GoDeleteResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filter)
        {
            return PrimaryMethodCaller().DeleteManyAsync(default, filter);
        }

        public IGoBulker<TDocument> NewBulker()
        {
            return new GoBulker<TDocument>(MongoCollection);
        }

        #region Primary Methods

        private IGoCollection<TDocument> PrimaryMethodCaller()
        {
            return this;
        }

        IEnumerable<TDocument> IGoCollection<TDocument>.Find(IClientSessionHandle session,
                                                             Expression<Func<TDocument, bool>> filter,
                                                             Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                                             GoFindOption<TDocument> goFindOption)
        {
            goFindOption ??= new GoFindOption<TDocument>();
            var builder = new GoProjectionBuilder<TDocument>();

            IFindFluent<TDocument, TDocument> findFluent = (session == null) switch
            {
                true => MongoCollection.Find(filter,
                                             new FindOptions
                                             {
                                                 AllowDiskUse = goFindOption.AllowDiskUse
                                             }),
                false => MongoCollection.Find(session,
                                              filter,
                                              new FindOptions
                                              {
                                                  AllowDiskUse = goFindOption.AllowDiskUse
                                              })

            };

            if (projection != null)
            {
                findFluent = findFluent.Project<TDocument>(projection?.Compile().Invoke(builder).MongoProjectionDefinition);
            }

            if (goFindOption.Sort != default)
            {
                findFluent = SortedFluent(findFluent, goFindOption.Sort);
            }

            return findFluent.Limit(goFindOption.Limit)
                             .Skip(goFindOption.Skip)
                             .ToEnumerable();
        }

        private static IFindFluent<TDocument, TDocument> SortedFluent(IFindFluent<TDocument, TDocument> findFluent,
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
                if (secondarySortRule.OrderType == OrderType.Ascending)
                {
                    orderedFluent = orderedFluent.ThenBy(secondarySortRule.KeySelector);
                }
                else if (secondarySortRule.OrderType == OrderType.Descending)
                {
                    orderedFluent = orderedFluent.ThenByDescending(secondarySortRule.KeySelector);
                }
            }

            return orderedFluent;
        }

        async Task<IEnumerable<TDocument>> IGoCollection<TDocument>.FindAsync(IClientSessionHandle session,
                                                                              Expression<Func<TDocument, bool>> filter,
                                                                              Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                                                              GoFindOption<TDocument> goFindOption)
        {

            goFindOption ??= new GoFindOption<TDocument>();
            var builder = new GoProjectionBuilder<TDocument>();
            var findOptions = new FindOptions<TDocument, TDocument>
            {
                AllowDiskUse = goFindOption.AllowDiskUse,
                Limit = goFindOption.Limit,
                Skip = goFindOption.Skip,
                Sort = ToSort(goFindOption.Sort)
            };

            if (projection != null)
            {
                findOptions.Projection = projection?.Compile().Invoke(builder).MongoProjectionDefinition;
            }

            if(session == null) return await(await MongoCollection.FindAsync(filter, findOptions)).ToListAsync();
            else return await (await MongoCollection.FindAsync(session, filter, findOptions)).ToListAsync();
        }

        TDocument IGoCollection<TDocument>.FindOne(IClientSessionHandle session,
                                                   Expression<Func<TDocument, bool>> filter,
                                                   Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                                   GoFindOption<TDocument> goFindOption)
        {
            goFindOption ??= new GoFindOption<TDocument>();
            goFindOption.Limit = 1;

            return PrimaryMethodCaller().Find(session, filter, projection, goFindOption).FirstOrDefault();
        }

        async Task<TDocument> IGoCollection<TDocument>.FindOneAsync(IClientSessionHandle session,
                                                                    Expression<Func<TDocument, bool>> filter,
                                                                    Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> projection,
                                                                    GoFindOption<TDocument> goFindOption)
        {
            if (goFindOption == null)
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

            return (await PrimaryMethodCaller().FindAsync(session, filter, projection, goFindOption)).FirstOrDefault();
        }

        void IGoCollection<TDocument>.InsertOne(IClientSessionHandle session,
                                                TDocument document)
        {
            if(session == null) MongoCollection.InsertOne(document);
            else MongoCollection.InsertOne(session, document);
        }

        Task IGoCollection<TDocument>.InsertOneAsync(IClientSessionHandle session,
                                                     TDocument document)
        {
            if(session == null) return MongoCollection.InsertOneAsync(document);
            else return MongoCollection.InsertOneAsync(session, document);
        }

        void IGoCollection<TDocument>.InsertMany(IClientSessionHandle session,
                                                 IEnumerable<TDocument> documents)
        {
            if (session == null) MongoCollection.InsertMany(documents);
            else MongoCollection.InsertMany(session, documents);
        }

        Task IGoCollection<TDocument>.InsertManyAsync(IClientSessionHandle session,
                                                      IEnumerable<TDocument> documents)
        {
            if (session == null) return MongoCollection.InsertManyAsync(documents);
            else return MongoCollection.InsertManyAsync(session, documents);
        }

        GoReplaceResult IGoCollection<TDocument>.ReplaceOne(IClientSessionHandle session,
                                                            Expression<Func<TDocument, bool>> filter,
                                                            TDocument document,
                                                            bool isUpsert)
        {
            ReplaceOneResult replaceResult;
            if (session == null)
            {
                replaceResult = MongoCollection.ReplaceOne(filter, document, new ReplaceOptions
                {
                    IsUpsert = isUpsert
                });
            }
            else
            {
                replaceResult = MongoCollection.ReplaceOne(session, filter, document, new ReplaceOptions
                {
                    IsUpsert = isUpsert
                });
            }

            return new GoReplaceResult(replaceResult);
        }

        async Task<GoReplaceResult> IGoCollection<TDocument>.ReplaceOneAsync(IClientSessionHandle session,
                                                                             Expression<Func<TDocument, bool>> filter,
                                                                             TDocument document,
                                                                             bool isUpsert)
        {
            ReplaceOneResult replaceResult;
            if (session == null)
            {
                replaceResult = await MongoCollection.ReplaceOneAsync(filter, document, new ReplaceOptions
                                                                                         {
                                                                                             IsUpsert = isUpsert
                                                                                         });
            }
            else
            {
                replaceResult = await MongoCollection.ReplaceOneAsync(session, filter, document, new ReplaceOptions
                {
                    IsUpsert = isUpsert
                });
            }

            return new GoReplaceResult(replaceResult);
        }


        TDocument IGoCollection<TDocument>.ReplaceOneAndRetrieve(IClientSessionHandle session,
                                                                 Expression<Func<TDocument, bool>> filter,
                                                                 TDocument document,
                                                                 GoReplaceOneAndRetrieveOptions<TDocument> options)
        {
            var projectBuilder = new GoProjectionBuilder<TDocument>();

            FindOneAndReplaceOptions<TDocument> findOneAndReplaceOptions = options == default ? default : new FindOneAndReplaceOptions<TDocument>
            {
                Projection = options.Projection?.Compile().Invoke(projectBuilder).MongoProjectionDefinition,
                ReturnDocument = options.ReturnDocument,
                IsUpsert = options.IsUpsert,
                Sort = ToSort(options?.Sort)
            };

            if(session == null)
            {
                return MongoCollection.FindOneAndReplace(filter,
                                                         document,
                                                         findOneAndReplaceOptions);
            }
            else
            {
                return MongoCollection.FindOneAndReplace(session,
                                                         filter,
                                                         document,
                                                         findOneAndReplaceOptions);
            }
        }

        private static SortDefinition<TDocument> ToSort(Expression<Func<GoSortBuilder<TDocument>, GoSortDefinition<TDocument>>> sort)
        {
            if (sort == default) return default;
            var goSortBuilder = new GoSortBuilder<TDocument>();
            var goSortDefinition = sort.Compile().Invoke(goSortBuilder);

            //deal primary
            SortDefinition<TDocument> sortDefinition = default;
            var primarySortRule = goSortDefinition._primarySortRule;
            if (primarySortRule.OrderType == OrderType.Ascending)
            {
                sortDefinition = Builders<TDocument>.Sort.Ascending(primarySortRule.KeySelector);
            }
            else if (primarySortRule.OrderType == OrderType.Descending)
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

        Task<TDocument> IGoCollection<TDocument>.ReplaceOneAndRetrieveAsync(IClientSessionHandle session,
                                                                            Expression<Func<TDocument, bool>> filter,
                                                                            TDocument document,
                                                                            GoReplaceOneAndRetrieveOptions<TDocument> options)
        {
            var projectBuilder = new GoProjectionBuilder<TDocument>();

            FindOneAndReplaceOptions<TDocument> findOneAndReplaceOptions = options == default ? default : new FindOneAndReplaceOptions<TDocument>
            {
                Projection = options.Projection?.Compile().Invoke(projectBuilder).MongoProjectionDefinition,
                ReturnDocument = options.ReturnDocument,
                IsUpsert = options.IsUpsert,
                Sort = ToSort(options?.Sort)
            };

            if (session == null)
            {
                return MongoCollection.FindOneAndReplaceAsync(filter,
                                                              document,
                                                              findOneAndReplaceOptions);
            }
            else
            {
                return MongoCollection.FindOneAndReplaceAsync(session,
                                                              filter,
                                                              document,
                                                              findOneAndReplaceOptions);
            }
        }

        long IGoCollection<TDocument>.Count(IClientSessionHandle session,
                                            Expression<Func<TDocument, bool>> filter)
        {
            if (session == null) return MongoCollection.CountDocuments(filter);
            else return MongoCollection.CountDocuments(session, filter);
        }

        Task<long> IGoCollection<TDocument>.CountAsync(IClientSessionHandle session,
                                                       Expression<Func<TDocument, bool>> filter)
        {
            if (session == null) return MongoCollection.CountDocumentsAsync(filter);
            else return MongoCollection.CountDocumentsAsync(session, filter);
        }

        GoUpdateResult IGoCollection<TDocument>.UpdateOne(IClientSessionHandle session,
                                                          Expression<Func<TDocument, bool>> filter,
                                                          Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                          bool isUpsert)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                           .Invoke(updateBuilder).MongoUpdateDefinition;
            UpdateResult mongoUpdateResult;
            
            if(session == null)
            {
                mongoUpdateResult = MongoCollection.UpdateOne(filter,
                                                              mongoUpdateDefinition,
                                                              new UpdateOptions
                                                              {
                                                                  IsUpsert = isUpsert
                                                              });
            }
            else
            {
                mongoUpdateResult = MongoCollection.UpdateOne(session,
                                                              filter,
                                                              mongoUpdateDefinition,
                                                              new UpdateOptions
                                                              {
                                                                  IsUpsert = isUpsert
                                                              });
            }
            
            return new GoUpdateResult(mongoUpdateResult);
        }

        async Task<GoUpdateResult> IGoCollection<TDocument>.UpdateOneAsync(IClientSessionHandle session,
                                                                           Expression<Func<TDocument, bool>> filter,
                                                                           Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                                           bool isUpsert)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                                               .Invoke(updateBuilder).MongoUpdateDefinition;
            UpdateResult mongoUpdateResult;
            if(session == null)
            {
                mongoUpdateResult = await MongoCollection.UpdateOneAsync(filter,
                                                                         mongoUpdateDefinition,
                                                                         new UpdateOptions
                                                                         {
                                                                             IsUpsert = isUpsert
                                                                         });
            }
            else
            {
                mongoUpdateResult = await MongoCollection.UpdateOneAsync(session,
                                                                         filter,
                                                                         mongoUpdateDefinition,
                                                                         new UpdateOptions
                                                                         {
                                                                             IsUpsert = isUpsert
                                                                         });
            }
            return new GoUpdateResult(mongoUpdateResult);
        }

        TDocument IGoCollection<TDocument>.UpdateOneAndRetrieve(IClientSessionHandle session,
                                                                Expression<Func<TDocument, bool>> filter,
                                                                Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                                GoUpdateOneAndRetrieveOptions<TDocument> options)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var projectBuilder = new GoProjectionBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                                               .Invoke(updateBuilder).MongoUpdateDefinition;

            FindOneAndUpdateOptions<TDocument> findOneAndUpdateOptions = options == default? default: new FindOneAndUpdateOptions<TDocument>
            {
                Projection = options.Projection?.Compile().Invoke(projectBuilder).MongoProjectionDefinition,
                ReturnDocument = options.ReturnDocument,
                IsUpsert = options.IsUpsert,
                Sort = ToSort(options?.Sort)
            };

            if (session == null)
            {
                 return MongoCollection.FindOneAndUpdate(filter,
                                                         mongoUpdateDefinition,
                                                         findOneAndUpdateOptions);
            }
            else
            {
                return MongoCollection.FindOneAndUpdate(session,
                                                        filter,
                                                        mongoUpdateDefinition,
                                                        findOneAndUpdateOptions);
            }
        }

        Task<TDocument> IGoCollection<TDocument>.UpdateOneAndRetrieveAsync(IClientSessionHandle session,
                                                                           Expression<Func<TDocument, bool>> filter,
                                                                           Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder,
                                                                           GoUpdateOneAndRetrieveOptions<TDocument> options)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var projectBuilder = new GoProjectionBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                                               .Invoke(updateBuilder).MongoUpdateDefinition;

            FindOneAndUpdateOptions<TDocument> findOneAndUpdateOptions = options == default ? default : new FindOneAndUpdateOptions<TDocument>
            {
                Projection = options.Projection?.Compile().Invoke(projectBuilder).MongoProjectionDefinition,
                ReturnDocument = options.ReturnDocument,
                IsUpsert = options.IsUpsert,
                Sort = ToSort(options?.Sort)
            };

            if (session == null)
            {
                return MongoCollection.FindOneAndUpdateAsync(filter,
                                                             mongoUpdateDefinition,
                                                             findOneAndUpdateOptions);
            }
            else
            {
                return MongoCollection.FindOneAndUpdateAsync(session,
                                                             filter,
                                                             mongoUpdateDefinition,
                                                             findOneAndUpdateOptions);
            }
        }

        GoUpdateResult IGoCollection<TDocument>.UpdateMany(IClientSessionHandle session,
                                                           Expression<Func<TDocument, bool>> filter,
                                                           Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                                               .Invoke(updateBuilder).MongoUpdateDefinition;
            UpdateResult mongoUpdateResult;
            if(session == null)
            {
                mongoUpdateResult = MongoCollection.UpdateMany(filter,
                                                               mongoUpdateDefinition);
            }
            else
            {
                mongoUpdateResult = MongoCollection.UpdateMany(session,
                                                               filter,
                                                               mongoUpdateDefinition);
            }
            return new GoUpdateResult(mongoUpdateResult);
        }

        async Task<GoUpdateResult> IGoCollection<TDocument>.UpdateManyAsync(IClientSessionHandle session,
                                                                            Expression<Func<TDocument, bool>> filter,
                                                                            Expression<Func<GoUpdateBuilder<TDocument>, GoUpdateDefinition<TDocument>>> updateDefinitionBuilder)
        {
            var updateBuilder = new GoUpdateBuilder<TDocument>();
            var mongoUpdateDefinition = updateDefinitionBuilder.Compile()
                                                               .Invoke(updateBuilder).MongoUpdateDefinition;
            UpdateResult mongoUpdateResult;
            if(session == null)
            {
                mongoUpdateResult = await MongoCollection.UpdateManyAsync(filter,
                                                                          mongoUpdateDefinition);
            }
            else
            {
                mongoUpdateResult = await MongoCollection.UpdateManyAsync(session,
                                                                          filter,
                                                                          mongoUpdateDefinition);
            }
            return new GoUpdateResult(mongoUpdateResult);
        }

        GoDeleteResult IGoCollection<TDocument>.DeleteOne(IClientSessionHandle session,
                                                          Expression<Func<TDocument, bool>> filter)
        {
            DeleteResult deleteResult;
            if(session == null)
            {
                deleteResult = MongoCollection.DeleteOne(filter);
            }
            else
            {
                deleteResult = MongoCollection.DeleteOne(session, filter);
            }
            return new GoDeleteResult(deleteResult);
        }

        async Task<GoDeleteResult> IGoCollection<TDocument>.DeleteOneAsync(IClientSessionHandle session,
                                                                           Expression<Func<TDocument, bool>> filter)
        {
            DeleteResult deleteResult;
            if (session == null)
            {
                deleteResult = await MongoCollection.DeleteOneAsync(filter);
            }
            else
            {
                deleteResult = await MongoCollection.DeleteOneAsync(session, filter);
            }
            return new GoDeleteResult(deleteResult);
        }

        TDocument IGoCollection<TDocument>.DeleteOneAndRetrieve(IClientSessionHandle session,
                                                                Expression<Func<TDocument, bool>> filter,
                                                                GoDeleteOneAndRetrieveOptions<TDocument> options)
        {
            FindOneAndDeleteOptions<TDocument, TDocument> findOneAndDeleteOptions = options == default ? default : new FindOneAndDeleteOptions<TDocument, TDocument>
            {
                Projection = options.Projection?.Compile().Invoke(new GoProjectionBuilder<TDocument>()).MongoProjectionDefinition,
                Sort = ToSort(options?.Sort)
            };

            if(session == null)
            {
                return MongoCollection.FindOneAndDelete(filter, findOneAndDeleteOptions);
            }
            else
            {
                return MongoCollection.FindOneAndDelete(session, filter, findOneAndDeleteOptions);
            }
        }

        Task<TDocument> IGoCollection<TDocument>.DeleteOneAndRetrieveAsync(IClientSessionHandle session,
                                                                           Expression<Func<TDocument, bool>> filter,
                                                                           GoDeleteOneAndRetrieveOptions<TDocument> options)
        {
            FindOneAndDeleteOptions<TDocument, TDocument> findOneAndDeleteOptions = options == default ? default : new FindOneAndDeleteOptions<TDocument, TDocument>
            {
                Projection = options.Projection?.Compile().Invoke(new GoProjectionBuilder<TDocument>()).MongoProjectionDefinition,
                Sort = ToSort(options?.Sort)
            };

            if(session == null)
            {
                return MongoCollection.FindOneAndDeleteAsync(filter, findOneAndDeleteOptions);
            }
            else
            {
                return MongoCollection.FindOneAndDeleteAsync(session, filter, findOneAndDeleteOptions);
            }
        }

        GoDeleteResult IGoCollection<TDocument>.DeleteMany(IClientSessionHandle session,
                                                           Expression<Func<TDocument, bool>> filter)
        {
            DeleteResult deleteResult;
            if (session == null)
            {
                deleteResult = MongoCollection.DeleteMany(filter);
            }
            else
            {
                deleteResult = MongoCollection.DeleteMany(session, filter);
            }
            return new GoDeleteResult(deleteResult);
        }

        async Task<GoDeleteResult> IGoCollection<TDocument>.DeleteManyAsync(IClientSessionHandle session,
                                                                            Expression<Func<TDocument, bool>> filter)
        {
            DeleteResult deleteResult;
            if (session == null)
            {
                deleteResult = await MongoCollection.DeleteManyAsync(filter);
            }
            else
            {
                deleteResult = await MongoCollection.DeleteManyAsync(session, filter);
            }
            return new GoDeleteResult(deleteResult);
        }
        #endregion
    }
}
