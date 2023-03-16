using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoGogo.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MongoGogo.Container
{
    /// <summary>
    /// A container to deal with the composition root of IGoContext and its corresponding IGoDatabse.IGoCollection and IGoCollection.
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
                OuterContainer.Registrations.AddRange(OuterContainer.RegisterContextAndDerived(context, option));
            }

            public void AddMongoContext(Type contextType, object contextInstance, LifeCycleOption option = default)
            {
                OuterContainer.Registrations.AddRange(OuterContainer.RegisterContextAndDerived(contextType, contextInstance, option));
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


        private List<GoRegistration> RegisterContextAndDerived(Type contextType,
                                                               object contextInstance,
                                                               LifeCycleOption option = default)
        {
            //check type
            if (contextType == null
                || !contextType.IsClass
                || !contextType.GetInterfaces()
                               .Any(@interface => @interface.GenericEquals(typeof(IGoContext<>)))) 
                throw new Exception($"{contextType.GetFriendlyName()} must be an implementation of IGoContext<>");

            List<GoRegistration> registrations = new List<GoRegistration>();

            option ??= new LifeCycleOption();

            //1. IMongoContext<TContext> → context 
            var iGoContextType = contextType;
            registrations.Add(new GoRegistration(registeredType: iGoContextType,
                                                 instance: contextInstance,
                                                 option.ContextLifeCycle));

            return RegisterDerived(registrations,
                                   contextType,
                                   option);
        }

        private List<GoRegistration> RegisterDerived(List<GoRegistration> registrations,
                                                     Type contextType,
                                                     LifeCycleOption option = default)
        {
            //alltypes
            IEnumerable<Type> AllTypes = AppDomain.CurrentDomain
                                                  .GetAssemblies()
                                                  .SelectMany(s => s.GetTypes());


            //2. 自動注入Database using reflection
            //IDatabase<TDatabase> →  Database<TContext, TDatabase> for all inner class TDatabase in TContext
            var dbTypes = contextType.GetNestedTypes();
            foreach (var dbType in dbTypes.Where(type => type.GetCustomAttribute<MongoDatabaseAttribute>() != null))
            {
                //get outer type: this class must be nested.
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
            //  (1)IMongoGoCollection<TDocument> → MongoCollection<TDatabase, TDocument> for all TMongoDatabase and corresponding TDocument
            //  (2)IGoCollection<TDocument> → <1> (high prior) some concrete class MyGoCollection<TDocuement> : GoCollectionAbstract
            //                                 <2> (low prior) default implementation, which is GoCollection

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
                //(1) IMongoCollection
                var collectionAttribute = collectionType.GetCustomAttribute<MongoCollectionAttribute>();
                var dbType = collectionAttribute.DbType;

                var serviceType = typeof(IMongoCollection<>).GetGenericTypeDefinition()
                                                            .MakeGenericType(collectionType);
                var implementType = typeof(MongoCollection<,>).GetGenericTypeDefinition()
                                                               .MakeGenericType(dbType, collectionType);

                registrations.Add(new GoRegistration(registeredType: serviceType,
                                                   mappedType: implementType,
                                                   option.MongoCollectionLifeCycle));

                //(2) IGoCollection
                var customImplementType = typeof(IGoCollection<>).GetGenericTypeDefinition()
                                                                 .MakeGenericType(collectionType);
                //this types shows all concrete type excepts GoCollection<TDocument> that implement IGoCollection<TDocument> 
                var customConcreteImplementTypes = AllTypes.Where(type => type.IsClass &&
                                                                          !type.IsAbstract &&
                                                                          (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(GoCollection<>)) &&
                                                                          type.GetInterfaces().Any(@interface => @interface == customImplementType))
                                                            .ToList();

                Type goCollectionServiceType = typeof(IGoCollection<>).GetGenericTypeDefinition().MakeGenericType(collectionType);
                Type goCollectionImplementType = customConcreteImplementTypes.Count switch
                {
                    0 => typeof(GoCollection<>).GetGenericTypeDefinition().MakeGenericType(collectionType),
                    1 => customConcreteImplementTypes.First(),
                    //the implementation mapping is one-to-one
                    _ => throw new Exception($"too many implementation: {string.Join(',', customConcreteImplementTypes.Select(type => type.GetFriendlyName()))}")
                };

                registrations.Add(new GoRegistration(registeredType: goCollectionServiceType,
                                                     mappedType: goCollectionImplementType,
                                                     option.GoCollectionLifeCycle));
            }

            return registrations;
        }

        private List<GoRegistration> RegisterContextAndDerived<TContext>(TContext context,
                                                                         LifeCycleOption option = default)
        where TContext : class, IGoContext<TContext>
        {
            List<GoRegistration> registrations = new List<GoRegistration>();
            option ??= new LifeCycleOption();

            //1. IMongoContext<TContext> → context 
            var iGoContextType = typeof(IGoContext<>).GetGenericTypeDefinition().MakeGenericType(typeof(TContext));
            registrations.Add(new GoRegistration(registeredType: iGoContextType,
                                                 instance: context,
                                                 option.ContextLifeCycle));

            return RegisterDerived(registrations,
                                   typeof(TContext),
                                   option);
        }
    }
}
