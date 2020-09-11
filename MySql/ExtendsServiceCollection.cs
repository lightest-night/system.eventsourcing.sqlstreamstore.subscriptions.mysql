using System;
using LightestNight.System.Data.MySql;
using LightestNight.System.EventSourcing.SqlStreamStore.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddEventStoreSubscriptions(this IServiceCollection services,
            Action<MySqlOptions> mysqlOptions, Action<EventSourcingOptions>? eventSourcingOptions = null)
            => services.AddMySqlEventStore(mysqlOptions, eventSourcingOptions)
                .AddHostedService<EventSubscription>();
    }
}