﻿using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PFM.Website.ExternalServices;
using PFM.Website.ExternalServices.InMemoryStorage;
using Serilog;
using Refit;
using AutoMapper;
using PFM.Website.Services.Mappers;
using Microsoft.IdentityModel.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PFM.Website.Monitoring.Metrics;
using PFM.Website.Monitoring.Tracing;

namespace PFM.Website.Configurations
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
		{
			var authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>() ?? new AuthOptions();

            IdentityModelEventSource.ShowPII = true;

            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = authOptions.Authority;

                    // Only in debug mode
                    options.RequireHttpsMetadata = false;

                    options.ClientId = authOptions.ClientId;
                    options.ClientSecret = authOptions.ClientSecret;
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UseTokenLifetime = false;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        ValidateIssuer = false
                    };

                    options.Events = new OpenIdConnectEvents
                    {
                        OnAccessDenied = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
		}
        
        public static IServiceCollection AddPfmApi(this IServiceCollection services, IConfiguration configuration, ApplicationSettings appSettings, bool isDevelopmentEnvironment)
        {
            if (!appSettings.PfmApiOptions.Enabled)
            {
                services
                    .AddSingleton<IExpenseTypeApi, ExpenseTypeInMemory>()
                    .AddSingleton<IBankApi, BankInMemory>()
                    .AddSingleton<ICountryApi, CountryInMemory>()
                    .AddSingleton<IBankAccountApi, BankAccountInMemory>()
                    .AddSingleton<ICurrencyApi, CurrencyInMemory>()
                    .AddSingleton<IIncomeApi, IncomeInMemory>()
                    .AddSingleton<ISavingApi, SavingInMemory>()
                    .AddSingleton<IAtmWithdrawApi, AtmWithdrawInMemory>()
                    .AddSingleton<IExpenseApi, ExpenseInMemory>()
                    .AddSingleton<IPaymentMethodApi, PaymentMethodInMemory>()
                    .AddSingleton<IMovementSummaryApi, MovementSummaryInMemory>()
                    .AddSingleton<IBudgetPlanApi, BudgetPlanInMemory>();
                return services;
            }

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

            var apiConfigs = appSettings.PfmApiOptions.EndpointUrl;
            services
                .AddPfmApiClient<IExpenseTypeApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ICountryApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBankApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ICurrencyApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBankAccountApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IIncomeApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ISavingApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IAtmWithdrawApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IExpenseApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IPaymentMethodApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IMovementSummaryApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBudgetPlanApi>(apiConfigs, refitSettings, httpClientHandler);

            return services;
        }

        private static IServiceCollection AddPfmApiClient<T>(this IServiceCollection services, string baseUrl, RefitSettings refitSettings, HttpClientHandler httpClientHandler)
            where T : class
        {
            services
               .AddRefitClient<T>(refitSettings)
               .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
               .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
               .AddHttpMessageHandler<AuthHeaderHandler>();

            return services;
        }

        public static IServiceCollection AddObjectMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<ModelToResponseProfile>();
                cfg.AddProfile<RequestToModelProfile>();
            });

            services.AddSingleton(configuration);
            services.AddSingleton<IMapper>(configuration.CreateMapper());

            return services;
        }
    }
}

