using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightestNight.System.Data.MySql;
using LightestNight.System.EventSourcing.Observers;
using LightestNight.System.EventSourcing.SqlStreamStore.MySql;
using LightestNight.System.ServiceResolution;
using Microsoft.Extensions.DependencyInjection;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddEventStoreSubscriptions(this IServiceCollection services,
            MySqlOptionsFactory mySqlOptionsFactory, Func<IServiceProvider, Type, IEventObserver>? observerTypeFactory = null,
            Action<EventSourcingOptions>? eventSourcingOptions = null, params Assembly[] assemblies)
        {
            services.AddServiceResolution();
            services.AddMySqlEventStore(mySqlOptionsFactory, eventSourcingOptions);
            
            var observerTypes = assemblies.SelectMany(assembly => assembly.GetLoadableTypes())
                .Where(typeof(IEventObserver).IsAssignableFrom);
            
            foreach (var observerType in observerTypes)
                services.AddSingleton(serviceProvider =>
                {
                    var serviceFactory = serviceProvider.GetRequiredService<ServiceFactory>();
                    return observerTypeFactory == null ? serviceFactory(observerType) as IEventObserver : observerTypeFactory(serviceProvider, observerType);
                });
            
            return services.AddHostedService<EventSubscription>();
        }
    }
}