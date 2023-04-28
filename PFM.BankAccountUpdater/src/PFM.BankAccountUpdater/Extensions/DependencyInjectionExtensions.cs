using EventStore.Client;
using PFM.BankAccountUpdater.Events;
using PFM.BankAccountUpdater.Events.Interface;
using PFM.BankAccountUpdater.Events.Settings;
using PFM.BankAccountUpdater.Handlers;
using PFM.BankAccountUpdater.Handlers.EventTypes;
using PFM.BankAccountUpdater.Handlers.Interfaces;
using Serilog;

namespace PFM.BankAccountUpdater.Extensions
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Set up logging.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonitoring(this IServiceCollection services, IConfiguration configuration, string environmentName)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", environmentName)
                .CreateLogger();

            services.AddSingleton(Log.Logger);
            return services;
        }

        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            var eventDispatcher = new EventDispatcher(Log.Logger);

            eventDispatcher.Register<BankAccountCreated>(e => (new BankAccountCreatedHandler(Log.Logger)).HandleEvent((BankAccountCreated)e));

            services.AddSingleton<IEventDispatcher>(eventDispatcher);

            return services;
        }

        public static IServiceCollection AddEventConsumerConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var eventStoreConnectionString = configuration.GetConnectionString("EventStoreConnection") ?? "";
            var settings = EventStoreClientSettings.Create(eventStoreConnectionString);
            var client = new EventStorePersistentSubscriptionsClient(settings);
            var subscriptionSettings = configuration.GetSection(nameof(EventStoreConsumerSettings)).Get<EventStoreConsumerSettings>() ?? new EventStoreConsumerSettings();

            services
                .AddSingleton(client)
                .AddSingleton(subscriptionSettings)
                .AddSingleton<IEventConsumer, EventConsumer>();

            return services;
        }
    }
}
