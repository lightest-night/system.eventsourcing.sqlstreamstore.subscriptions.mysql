using System;
using LightestNight.System.Data.MySql;
using LightestNight.System.EventSourcing.SqlStreamStore.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddEventStoreSubscriptions(this IServiceCollection services,
            MySqlOptionsFactory mySqlOptionsFactory, Action<EventSourcingOptions>? eventSourcingOptions = null)
            => services.AddMySqlEventStore(mySqlOptionsFactory, eventSourcingOptions)
                .AddHostedService<EventSubscription>();
    }
}