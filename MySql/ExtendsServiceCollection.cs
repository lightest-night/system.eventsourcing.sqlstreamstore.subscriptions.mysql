﻿using System;
using LightestNight.System.Data.MySql;
using Microsoft.Extensions.DependencyInjection;
using SqlStreamStore;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddEventStoreSubscriptions(this IServiceCollection services,
            Action<MySqlOptions> mysqlOptions, Action<EventSourcingOptions>? eventSourcingOptions = null)
        {
            var options = new EventSourcingOptions();
            eventSourcingOptions?.Invoke(options);

            services.AddMySqlData(mysqlOptions);

            var serviceProvider = services.BuildServiceProvider();
            if (!(serviceProvider.GetService<IStreamStore>() is MySqlStreamStore))
            {
                services.AddSingleton<IStreamStore>(sp =>
                {
                    var connectionString = sp.GetRequiredService<IMySqlConnection>().Build().ConnectionString;
                    return new MySqlStreamStore(new MySqlStreamStoreSettings(connectionString));
                });
            }

            return services.AddHostedService<EventSubscription>();
        } 
    }
}