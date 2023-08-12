using EventStore.Client;
using PFM.Bank.Event.Contracts;
using PFM.MovementAggregator.Caches;
using PFM.MovementAggregator.Caches.Interfaces;
using PFM.MovementAggregator.Events;
using PFM.MovementAggregator.Events.Interface;
using PFM.MovementAggregator.Events.Settings;
using PFM.MovementAggregator.ExternalServices.AuthApi;
using PFM.MovementAggregator.ExternalServices.AuthApi.Implementations;
using PFM.MovementAggregator.Handlers;
using PFM.MovementAggregator.Handlers.Interfaces;
using PFM.MovementAggregator.Services;
using PFM.MovementAggregator.Services.Interfaces;
using PFM.MovementAggregator.Settings;
using Serilog;

namespace PFM.MovementAggregator.Extensions
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            var eventDispatcher = new EventDispatcher(Log.Logger, services);

            services.AddSingleton<IHandler<BankAccountDebited>, BankAccountDebitedHandler>();
            services.AddSingleton<IHandler<BankAccountCredited>, BankAccountCreditedHandler>();

            eventDispatcher.Register<BankAccountDebited>();
            eventDispatcher.Register<BankAccountCredited>();

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

        /// <summary>
        /// Set up authentication and authorization.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfigs = new AuthApiSettings();
            configuration.Bind("AuthApi", authConfigs);

            services
                .AddSingleton(authConfigs)
                .AddTransient<AuthHeaderHandler>()
                .AddSingleton<IAuthTokenStore, AuthTokenStore>()
                .AddSingleton<ITokenCache, TokenCache>();

            services.AddHttpClient<IAuthApi, KeycloakAuthApi>(client =>
            {
                client.BaseAddress = new Uri(authConfigs.EndpointUrl ?? "https://localhost");
            });

            return services;
        }
    }
}
