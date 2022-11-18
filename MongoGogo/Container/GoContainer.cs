using MongoGogo.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MongoGogo.Container
{
    /// <summary>
    /// A container to deal with the composition root of IGoContext and its corresponding IGoDatabse.IGoCollection and IGoRepository.
    /// </summary>
    public class GoContainer
    {
        public List<GoRegistration> Registrations { get; private set; }

        private ServiceBuilder _Builder;

        public GoContainer()
        {
            Registrations = new List<GoRegistration>();
            _Builder = new ServiceBuilder(this);
        }

        public class ServiceBuilder
        {
            private readonly GoContainer OuterContainer;

            internal ServiceBuilder(GoContainer goContainer)
            {
                this.OuterContainer = goContainer;
            }

            public void AddMongoContext<TContext>(TContext context, LifeCycleOption option = default) where TContext : class, IGoContext<TContext>
            {
                OuterContainer.Registrations.AddRange(OuterContainer.RegisterDerived(context, option));
            }
        }

        /// <summary>
        /// Build the composite root.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public GoContainer Composite(Action<ServiceBuilder> option)
        {
            option.Invoke(_Builder);
            return this;
        }

        private List<GoRegistration> RegisterDerived<TContext>(TContext context,
                                                               LifeCycleOption option = default)
            where TContext : class, IGoContext<TContext>
        {
            List<GoRegistration> registrations = new List<GoRegistration>();

            option ??= new LifeCycleOption();

            //alltypes
            IEnumerable<Type> AllTypes = AppDomain.CurrentDomain
                                                  .GetAssemblies()
                                                  .SelectMany(s => s.GetTypes());

            //1. IMongoContext<TContext> → context 
            var iGoContextType = typeof(IGoContext<>).GetGenericTypeDefinition().MakeGenericType(typeof(TContext));
            registrations.Add(new GoRegistration(registeredType: iGoContextType,
                                               instance: context,
                                               option.ContextLifeCycle));


            //2. 自動注入Database using reflection
            //IDatabase<TDatabase> →  Database<TContext, TDatabase> for all inner class TDatabase in TContext
            var dbTypes = typeof(TContext).GetNestedTypes();
            foreach (var dbType in dbTypes.Where(type => type.GetCustomAttribute<MongoDatabaseAttribute>() != null))
            {
                //get outer type: this class must be nested.
                var contextType = typeof(TContext);

                if (contextType.GetInterfaces()
                               .Count(@interface => @interface.GenericEquals(typeof(IGoContext<>))) != 1)
                {
                    //this class must be an inner class of IMongoContext<>
                    throw new Exception("not implement IMongoContext"); //錯訊再補
                }

                //make interface
                var serviceType = typeof(IGoDatabase<>).GetGenericTypeDefinition()
                                                       .MakeGenericType(dbType);

                var implementType = typeof(GoDatabase<,>).GetGenericTypeDefinition()
                                                         .MakeGenericType(contextType, dbType);

                registrations.Add(new GoRegistration(registeredType: serviceType,
                                                   mappedType: implementType,
                                                   option.DatabaseLifeCycle));
            }


            //3. inject IGoCollections using reflection
            //  (1)IGoCollection<TDocument> → GoCollection<TDatabase, TDocument> for all TMongoDatabase and corresponding TDocument
            //  (2)IGoRepository<TDocument> → <1> (high prior) some concrete class MyRepository<TDocuement> : GoRepositoryAbstract
            //                                 <2> (low prior) default implementation, which is GoRepository

            //todo: not a perfect algorithm. Should be dealt globally
            var collDict = AllTypes.Where(type => type.GetCustomAttribute<MongoCollectionAttribute>() != null)
                                    .GroupBy(type =>
                                    {
                                        var mongoCollectionAttribute = type.GetCustomAttribute<MongoCollectionAttribute>();
                                        return mongoCollectionAttribute.DbType;
                                    })
                                    .ToDictionary(group => group.Key,
                                                  group => group.ToList());

            foreach (var collectionType in collDict.Where(keyValue => dbTypes.Contains(keyValue.Key))
                                                   .SelectMany(dict => dict.Value))
            {
                //(1) IGoCollection
                var collectionAttribute = collectionType.GetCustomAttribute<MongoCollectionAttribute>();
                var dbType = collectionAttribute.DbType;

                var serviceType = typeof(Connection.IGoCollection<>).GetGenericTypeDefinition()
                                                                  .MakeGenericType(collectionType);
                var implementType = typeof(GoCollection<,>).GetGenericTypeDefinition()
                                                               .MakeGenericType(dbType, collectionType);

                registrations.Add(new GoRegistration(registeredType: serviceType,
                                                   mappedType: implementType,
                                                   option.CollectionLifeCycle));

                //(2) IGoRepository
                var customImplementType = typeof(IGoRepository<>).GetGenericTypeDefinition()
                                                                 .MakeGenericType(collectionType);
                //this types shows all concrete type excepts GoRepository<TDocument> that implement IGoRepository<TDocument> 
                var customConcreteImplementTypes = AllTypes.Where(type => type.IsClass &&
                                                                          !type.IsAbstract &&
                                                                          (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(GoRepository<>)) &&
                                                                          type.GetInterfaces().Any(@interface => @interface == customImplementType))
                                                            .ToList();

                Type repositoryServiceType = typeof(IGoRepository<>).GetGenericTypeDefinition().MakeGenericType(collectionType);
                Type repositoryImplementType = customConcreteImplementTypes.Count switch
                {
                    0 => typeof(GoRepository<>).GetGenericTypeDefinition().MakeGenericType(collectionType),
                    1 => customConcreteImplementTypes.First(),
                    //the implementation mapping is one-to-one
                    _ => throw new Exception($"too many implementation: {string.Join(',', customConcreteImplementTypes.Select(type => type.GetFriendlyName()))}")
                };

                registrations.Add(new GoRegistration(registeredType: repositoryServiceType,
                                                   mappedType: repositoryImplementType,
                                                   option.RepositoryLifeCycle));
            }

            return registrations;
        }
    }
}
