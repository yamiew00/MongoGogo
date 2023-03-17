﻿using Microsoft.Extensions.DependencyInjection;
using MongoGogo.Connection;
using MongoGogo.Container;

namespace MongoGogo
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMongoContext<TContext>(this IServiceCollection serviceCollection,
                                                            TContext mongoContext,
                                                            LifeCycleOption option = default)
            where TContext : class, IGoContext<TContext>
        {
            //build the composite root of Context
            GoContainer goContainer = new GoContainer().Build(builder => builder.AddMongoContext(mongoContext, option));

            foreach (var registration in goContainer.Registrations)
            {
                if(registration.LifeTime == LifeCycleType.Singleton)
                {
                    serviceCollection.AddSingleton(serviceType: registration.RegisteredType,
                                                   implementationInstance: goContainer.Resolve(registration.RegisteredType));
                }
                else if(registration.LifeTime == LifeCycleType.Scoped)
                {
                    serviceCollection.AddScoped(serviceType: registration.RegisteredType,
                                                implementationType: registration.MappedType);
                }
                else if(registration.LifeTime == LifeCycleType.Transient)
                {
                    serviceCollection.AddTransient(serviceType: registration.RegisteredType,
                                                       implementationType: registration.MappedType);
                }
            }
            return serviceCollection;
        }
    }
}
