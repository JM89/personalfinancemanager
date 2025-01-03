﻿using EventStore.Client;
using PFM.Bank.Event.Contracts;
using PFM.BankAccountUpdater.Caches;
using PFM.BankAccountUpdater.Caches.Interfaces;
using PFM.BankAccountUpdater.Events;
using PFM.BankAccountUpdater.Events.Interface;
using PFM.BankAccountUpdater.Events.Settings;
using PFM.BankAccountUpdater.ExternalServices.AuthApi;
using PFM.BankAccountUpdater.ExternalServices.AuthApi.Implementations;
using PFM.BankAccountUpdater.ExternalServices.BankApi;
using PFM.BankAccountUpdater.Handlers;
using PFM.BankAccountUpdater.Handlers.Interfaces;
using PFM.BankAccountUpdater.Services;
using PFM.BankAccountUpdater.Services.Interfaces;
using PFM.BankAccountUpdater.Settings;
using Refit;
using Serilog;
using System.Text.Json;

namespace PFM.BankAccountUpdater.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IBankAccountService, BankAccountService>();

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

            services.AddHttpClient<IAuthApi, KeycloakAuthApi>(client => {
                client.BaseAddress = new Uri(authConfigs.EndpointUrl ?? "https://localhost");
            });

            return services;
        }

        public static IServiceCollection AddBankApi(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnvironment)
        {
            var apiConfigs = configuration["BankApi:EndpointUrl"];
            if (apiConfigs == null)
                throw new Exception("DI exception: Bank API config was not found");

            var refitSettings = new RefitSettings()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Converters =
                        {
                            new ObjectToInferredTypesConverter(),
                        }
                    }
                ),
                ExceptionFactory = httpResponse =>
                {
                    return Task.FromResult<Exception?>(null);
                }
            };

            var httpClientHandler = !isDevelopmentEnvironment ? new HttpClientHandler() : new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true };

            services
                .AddRefitClient<IBankAccountApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfigs))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>();

            return services;
        }
    }
}
