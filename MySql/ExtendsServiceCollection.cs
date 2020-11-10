using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightestNight.System.Data.MySql;
using LightestNight.System.EventSourcing.Observers;
using LightestNight.System.EventSourcing.SqlStreamStore.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddEventStoreSubscriptions(this IServiceCollection services,
            MySqlOptionsFactory mySqlOptionsFactory, Func<IServiceProvider, Type, IEventObserver>? observerTypeFactory = null,
            Action<EventSourcingOptions>? eventSourcingOptions = null, params Assembly[] assemblies)
        {
            services.AddMySqlEventStore(mySqlOptionsFactory, eventSourcingOptions);
            
            var observerTypes = assemblies.SelectMany(assembly => assembly.GetLoadableTypes())
                .Where(typeof(IEventObserver).IsAssignableFrom);
            foreach (var observerType in observerTypes)
                services.AddSingleton(serviceProvider =>
                {
                    if (observerTypeFactory == null)
                        return Activator.CreateInstance(observerType) as IEventObserver;
            
                    return observerTypeFactory(serviceProvider, observerType);
                });
            
            return services.AddHostedService<EventSubscription>();
        }

        public static IServiceCollection AddEventStoreSubscriptions(this IServiceCollection services,
            MySqlOptionsFactory mySqlOptionsFactory, IEnumerable<IEventObserver> observerInstances,
            Action<EventSourcingOptions>? eventSourcingOptions = null)
        {
            services.AddMySqlEventStore(mySqlOptionsFactory, eventSourcingOptions);

            foreach (var observer in observerInstances)
                services.AddSingleton(observer);

            return services.AddHostedService<EventSubscription>();
        }
    }
}