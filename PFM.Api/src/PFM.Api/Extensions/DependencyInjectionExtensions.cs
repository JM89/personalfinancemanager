﻿using System.Reflection;
using EventStore.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PFM.Api.Settings;
using PFM.Services.Caches;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events;
using PFM.Services.Events.Interfaces;
using Refit;
using System.Text.Json;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using Swashbuckle.AspNetCore.Filters;

namespace PFM.Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Set up authentication and authorization.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="authOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, AuthOptions authOptions)
        {
            if (!authOptions.Enabled)
                return services;
            
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            if (authOptions?.Authority == null)
                throw new Exception("DI exception: Auth API config was not found");

            Console.WriteLine($"Authority: {authOptions.Authority}");

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Authority;
                    options.Audience = "account";
                    options.RequireHttpsMetadata = authOptions.RequireHttpsMetadata;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = authOptions.ValidateIssuer
                    };
                });
                
            return services;
        }

        public static IServiceCollection AddBankApi(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnvironment)
        {
            var endpointUrl = configuration["BankApi:EndpointUrl"];
            if (endpointUrl == null)
                throw new Exception("DI exception: Bank API config was not found");

            Console.WriteLine($"Bank API endpoint: {endpointUrl}");

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
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.IBankAccountApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(endpointUrl))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>();
            services.AddSingleton<IBankAccountCache, BankAccountCache>();

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.IBankApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(endpointUrl))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>();
            services.AddSingleton<IBankCache, BankCache>();

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.ICurrencyApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(endpointUrl))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>();
            services.AddSingleton<ICurrencyCache, CurrencyCache>();

            services
                .AddRefitClient<PFM.Services.ExternalServices.BankApi.ICountryApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(endpointUrl))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>(); 
            services.AddSingleton<ICountryCache, CountryCache>();

            return services;
        }

        public static IServiceCollection AddPensionApi(this IServiceCollection services, ApiOptions apiOptions, bool isDevelopmentEnvironment)
        {
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
                ExceptionFactory = httpResponse => Task.FromResult<Exception?>(null)
            };

            var httpClientHandler = !isDevelopmentEnvironment ? new HttpClientHandler() : new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true };

            services
                .SetApiClient<IPensionApi, IPensionCache, PensionCache>(httpClientHandler, apiOptions.EndpointUrl, refitSettings)
                .SetApiClient<IIncomeTaxReportApi, IIncomeTaxReportCache, IncomeTaxReportCache>(httpClientHandler, apiOptions.EndpointUrl, refitSettings);

            return services;
        }

        private static IServiceCollection SetApiClient<TApi, TICache, TCache>(this IServiceCollection services, HttpClientHandler httpClientHandler, string endpointUrl, RefitSettings refitSettings) 
            where TApi : class
            where TICache : class
            where TCache : class, TICache
        {
            services
                .AddRefitClient<TApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(endpointUrl))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>();
            services.AddSingleton<TICache, TCache>();

            return services;
        }

        /// <summary>
        /// Set up swagger.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="applicationSettings"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDefinition(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = applicationSettings.ApplicationName,
                    Description = applicationSettings.ShortDescription,
                    Version = Program.AssemblyVersion,
                });

                options.CustomSchemaIds( type => type.ToString() );
                
                options.ExampleFilters();
                
                if (!applicationSettings.AuthOptions.Enabled)
                    return;
                
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
