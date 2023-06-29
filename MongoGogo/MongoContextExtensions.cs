using MongoGogo;
using MongoGogo.Connection;
using MongoGogo.Container;

namespace Microsoft.Extensions.DependencyInjection
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

            //todo: not a good way to manage serviceProvider
            goContainer.NeedIServiceProvider = false;   

            foreach (var registration in goContainer.Registrations)
            {
                if(registration.LifeTime == LifeCycleType.Singleton)
                {
                    if(registration.Instance != null)
                    {
                        serviceCollection.AddSingleton(serviceType: registration.RegisteredType,
                                                       implementationInstance: goContainer.Resolve(registration.RegisteredType));
                    }
                    else
                    {
                        serviceCollection.AddSingleton(serviceType: registration.RegisteredType,
                                                       implementationType: registration.MappedType);
                    }
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
