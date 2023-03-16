using Microsoft.Extensions.DependencyInjection;
using MongoGogo.Connection;
using MongoGogo.Container;

namespace MongoGogo
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMongoContext2<TContext>(this IServiceCollection serviceCollection,
                                                            TContext mongoContext,
                                                            LifeCycleOption option = default)
            where TContext : class, IGoContext<TContext>
        {
            //build the composite root of Context
            GoContainer goContainer = new GoContainer().Build(builder => builder.AddMongoContext(mongoContext, option));

            //todo: lifeoption not work
            foreach (var registration in goContainer.Registrations)
            {
                if (registration.Instance != null)
                {
                    //singleton
                    serviceCollection.AddSingleton(serviceType: registration.RegisteredType,
                                                   implementationInstance: registration.Instance);
                }
                else
                {
                    var lifeTime = registration.LifeTime;
                    if (lifeTime == LifeCycleType.Transient)
                    {
                        serviceCollection.AddTransient(serviceType: registration.RegisteredType,
                                                       implementationType: registration.MappedType);
                    }
                    else
                    {
                        serviceCollection.AddScoped(serviceType: registration.RegisteredType,
                                                    implementationType: registration.MappedType);
                    }
                }
            }
            return serviceCollection;
        }
    }
}
