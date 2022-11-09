﻿using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoGogo.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MongoGogo
{
    public static class ServiceCollectionExtension
    {
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
            where TContext : IGoContext<TContext>
        {
            return serviceCollection.AddMongoContext<TContext>(_ => mongoContext,
                                                               option);
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
                                                                   Func<IServiceProvider, object> implementationFactory,
                                                                   LifeCycleOption option = default)
    where TContext : IGoContext<TContext>
        {
            //lifecycle option
            option ??= new LifeCycleOption();

            //di of this implemented class, with scope lifecycle
            serviceCollection.AddScoped(typeof(TContext), implementationFactory);

            //alltypes
            IEnumerable<Type> AllTypes = AppDomain.CurrentDomain
                                                  .GetAssemblies()
                                                  .SelectMany(s => s.GetTypes());

            //1. 自動注入所有自建的IMongoContext<>
            //ex: IMongoContext<TContext> → TContext for all TContext
            //todo: 用Name去判斷會有重名造成的錯誤
            foreach (var mongoContextType in AllTypes.Where(type => type.BaseType?.Name == typeof(GoContext<>).Name))
            {
                var serviceType = typeof(IGoContext<>).GetGenericTypeDefinition().MakeGenericType(mongoContextType);
                var implementType = mongoContextType;

                //自動implement的寫法是用factory去resolve出對應型別
                var contextLifeCycle = option.ContextLifeCycle;
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
                            .Count(@interface => @interface.Name == typeof(IGoContext<>).Name) != 1)
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


            //3. 注入Collections using reflection
            //ICollection<TDocument> → Collection<TMongoDatabase, TDocument> for all TMongoDatabase and corresponding TDocument
            foreach (var collectionType in AllTypes.Where(type => type.GetCustomAttribute<MongoCollectionAttribute>() != null))
            {
                var collectionAttribute = collectionType.GetCustomAttribute<MongoCollectionAttribute>();
                var dbType = collectionAttribute.DbType;

                var serviceType = typeof(Connection.IGoCollection<>).GetGenericTypeDefinition()
                                                                  .MakeGenericType(collectionType);
                var implementType = typeof(GoCollection<,>).GetGenericTypeDefinition()
                                                               .MakeGenericType(dbType, collectionType);

                serviceCollection.AddService(option.CollectionLifeCycle,
                                             serviceType,
                                             implementType);
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
    }
}
