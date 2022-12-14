using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoGogo.Connection;
using MongoGogo.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MongoGogo
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMongoContext(this IServiceCollection serviceCollection,
                                                         Type contextType,
                                                         object mongoContext,
                                                         LifeCycleOption option = default)
        {
            return serviceCollection.AddMongoContext(contextType, _ => mongoContext, option);
        }

        public static IServiceCollection AddMongoContext(this IServiceCollection serviceCollection,
                                                         Type contextType,
                                                         Func<IServiceProvider, object> implementationFactory,
                                                         LifeCycleOption option = default)
        {
            //lifecycle option
            option ??= new LifeCycleOption();

            serviceCollection.AddService(option.ContextLifeCycle, contextType, implementationFactory);
            return serviceCollection.RegisterElementsInContext(option);
        }


        /// <summary>
        /// Add an IGoContext to .net dependency injection container with scope lifecycle.
        /// </summary>
        /// <typeparam name="TContext">must be in form of IMongoContext<TContext> </typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="mongoContext">the instance of the mongoContext</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoContext<TContext>(this IServiceCollection serviceCollection,
                                                                   TContext mongoContext,
                                                                   LifeCycleOption option = default)
            where TContext : class, IGoContext<TContext>
        {
            return serviceCollection.AddMongoContext(_ => mongoContext,option);
        }

        /// <summary>
        /// Add an IGoContext to .net dependency injection container with scope lifecycle.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AddMongoContext<TContext>(this IServiceCollection serviceCollection,
                                                                   Func<IServiceProvider, TContext> implementationFactory,
                                                                   LifeCycleOption option = default)
                where TContext : class, IGoContext<TContext>
        {

            //lifecycle option
            option ??= new LifeCycleOption();

            //di of this implemented class, with option lifecycle
            serviceCollection.AddService(option.ContextLifeCycle, typeof(TContext), implementationFactory);

            return serviceCollection.RegisterElementsInContext(option);
        }

        private static IServiceCollection RegisterElementsInContext(this IServiceCollection serviceCollection, 
                                                                    LifeCycleOption option = default)
        {
            //alltypes
            IEnumerable<Type> AllTypes = AppDomain.CurrentDomain
                                                  .GetAssemblies()
                                                  .SelectMany(s => s.GetTypes());

            //1. 自動注入所有自建的IMongoContext<>
            //ex: IMongoContext<TContext> → TContext for all TContext
            foreach (var mongoContextType in AllTypes.Where(type => type.BaseType.GenericEquals(typeof(GoContext<>))))
            {
                var serviceType = typeof(IGoContext<>).GetGenericTypeDefinition().MakeGenericType(mongoContextType);
                var implementType = mongoContextType;

                //自動implement的寫法是用factory去resolve出對應型別
                serviceCollection.AddService(option.ContextLifeCycle,
                                             serviceType,
                                             serviceProvider => serviceProvider.GetService(implementType));
            }

            //2. 自動注入Database using reflection
            //IDatabase<TDatabase> →  Database<TContext, TDatabase> for all TContext and corresponding inner class TDatabase
            foreach (var dbType in AllTypes.Where(type => type.GetCustomAttribute<MongoDatabaseAttribute>() != null))
            {
                //get outer type: this class must be nested.
                var contextType = dbType.DeclaringType;
                if (contextType == null) throw new Exception("not a inner class"); //錯訊再補

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

                serviceCollection.AddService(option.DatabaseLifeCycle,
                                             serviceType,
                                             implementType);
            }


            //3. inject IGoCollections using reflection
            //  (1)IGoCollection<TDocument> → GoCollection<TDatabase, TDocument> for all TMongoDatabase and corresponding TDocument
            //  (2)IGoRepository<TDocument> → <1> (high prior) some concrete class MyRepository<TDocuement> : GoRepositoryAbstract
            //                                 <2> (low prior) default implementation, which is GoRepository
            foreach (var collectionType in AllTypes.Where(type => type.GetCustomAttribute<MongoCollectionAttribute>() != null))
            {
                //(1) IGoCollection
                var collectionAttribute = collectionType.GetCustomAttribute<MongoCollectionAttribute>();
                var dbType = collectionAttribute.DbType;

                var serviceType = typeof(Connection.IGoCollection<>).GetGenericTypeDefinition()
                                                                  .MakeGenericType(collectionType);
                var implementType = typeof(GoCollection<,>).GetGenericTypeDefinition()
                                                               .MakeGenericType(dbType, collectionType);

                serviceCollection.AddService(option.CollectionLifeCycle,
                                             serviceType,
                                             implementType);

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

                serviceCollection.AddService(option.RepositoryLifeCycle,
                                             repositoryServiceType,
                                             repositoryImplementType);
            }

            return serviceCollection;
        }

        private static IServiceCollection AddService(this IServiceCollection serviceCollection,
                                                     LifeCycleType lifeCycleType,
                                                     Type serviceType,
                                                     Func<IServiceProvider, object> implementationFactory)
        {
            if (lifeCycleType == LifeCycleType.Singleton)
            {
                serviceCollection.AddSingleton(serviceType, implementationFactory);
            }
            else if (lifeCycleType == LifeCycleType.Scoped)
            {
                serviceCollection.AddScoped(serviceType, implementationFactory);
            }
            else if (lifeCycleType == LifeCycleType.Transient)
            {
                serviceCollection.AddTransient(serviceType, implementationFactory);
            }
            else
            {
                throw new NotImplementedException();
            }
            return serviceCollection;
        }

        private static IServiceCollection AddService<TService>(this IServiceCollection serviceCollection,
                                                               LifeCycleType lifeCycleType,
                                                               Type serviceType,
                                                               Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (lifeCycleType == LifeCycleType.Singleton)
            {
                serviceCollection.AddSingleton(implementationFactory);
            }
            else if (lifeCycleType == LifeCycleType.Scoped)
            {
                serviceCollection.AddScoped(implementationFactory);
            }
            else if (lifeCycleType == LifeCycleType.Transient)
            {
                serviceCollection.AddTransient(implementationFactory);
            }
            else
            {
                throw new NotImplementedException();
            }
            return serviceCollection;
        }

        private static IServiceCollection AddService(this IServiceCollection serviceCollection,
                                                     LifeCycleType lifeCycleType,
                                                     Type serviceType,
                                                     Type implenmentType)
        {
            if (lifeCycleType == LifeCycleType.Singleton)
            {
                serviceCollection.AddSingleton(serviceType, implenmentType);
            }
            else if (lifeCycleType == LifeCycleType.Scoped)
            {
                serviceCollection.AddScoped(serviceType, implenmentType);
            }
            else if (lifeCycleType == LifeCycleType.Transient)
            {
                serviceCollection.AddTransient(serviceType, implenmentType);
            }
            else
            {
                throw new NotImplementedException();
            }
            return serviceCollection;
        }

        public static IServiceCollection AddMongoContainer(this IServiceCollection serviceCollection,
                                                           GoContainer goContainer)
        {
            var registrations = goContainer.Registrations;

            foreach (var registration in registrations)
            {
                var lifeCycle = registration.LifeTime;
                if(registration.Instance == null)
                {
                    serviceCollection.AddService(lifeCycleType: lifeCycle,
                                                 serviceType: registration.RegisteredType,
                                                 implenmentType: registration.MappedType);
                    continue;
                }

                serviceCollection.AddService(lifeCycleType: lifeCycle,
                                             serviceType: registration.RegisteredType,
                                             implementationFactory: _ => registration.Instance);
            }

            return serviceCollection;
        }
    }
}
