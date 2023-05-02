using EventStore.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using PFM.Services.Caches;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events;
using PFM.Services.Events.Interfaces;
using PFM.Services.ExternalServices.AuthApi;
using Refit;
using Serilog;

namespace PFM.Api.Extensions
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

        /// <summary>
        /// Set up authentication and authorization.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationApiConfigs = configuration["AuthApi:EndpointUrl"];
            if (authenticationApiConfigs == null)
                throw new Exception("DI exception: Authentication API config was not found");

            services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(authenticationApiConfigs));

            services.AddMvc(o =>
            {
                var authenticationApi = services.BuildServiceProvider().GetService<IAuthApi>();
                if (authenticationApi == null)
                    throw new Exception("DI exception: Authentication API was not injected");

                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
                o.Filters.Add(new Api.Filters.DelegateToAuthApiAuthorizeFilter(policy, Log.Logger, authenticationApi));
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                });
            return services;
        }

        public static IServiceCollection AddBankApi(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfigs = configuration["BankApi:EndpointUrl"];
            if (apiConfigs == null)
                throw new Exception("DI exception: Bank API config was not found");

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.IBankAccountApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfigs));
            services.AddSingleton<IBankAccountCache, BankAccountCache>();

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.IBankApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfigs));
            services.AddSingleton<IBankCache, BankCache>();

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.ICurrencyApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfigs));
            services.AddSingleton<ICurrencyCache, CurrencyCache>();

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.ICountryApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfigs));
            services.AddSingleton<ICountryCache, CountryCache>();

            return services;
        }

        /// <summary>
        /// Set up swagger.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDefinition(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddEventPublisherConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var eventStoreConnectionString = configuration.GetConnectionString("EventStoreConnection");
            var settings = EventStoreClientSettings.Create(eventStoreConnectionString);
            var client = new EventStoreClient(settings);

            services
                .AddSingleton(client)
                .AddSingleton<IEventPublisher, EventPublisher>();

            return services;
        }
    }
}
