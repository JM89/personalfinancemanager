using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PFM.Website.ExternalServices;
using PFM.Website.ExternalServices.InMemoryStorage;
using Serilog;
using Refit;
using AutoMapper;
using PFM.Website.Services.Mappers;

namespace PFM.Website.Configurations
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
		{
			var authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>() ?? new AuthOptions();

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
                    options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name" };

                    options.SaveTokens = true;

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

        public static IServiceCollection AddPfmApi(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnvironment)
        {
            var useApi = configuration.GetValue<bool>("UsePfmApi");

            if (!useApi)
            {
                services
                    .AddSingleton<IExpenseTypeApi, ExpenseTypeInMemory>()
                    .AddSingleton<IBankApi, BankInMemory>()
                    .AddSingleton<ICountryApi, CountryInMemory>()
                    .AddSingleton<IBankAccountApi, BankAccountInMemory>()
                    .AddSingleton<ICurrencyApi, CurrencyInMemory>();
                return services;
            }

            var apiConfigs = configuration["PfmApi:EndpointUrl"];
            if (apiConfigs == null)
                throw new Exception("DI exception: PFM API config was not found");

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
                .AddPfmApiClient<IExpenseTypeApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ICountryApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBankApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ICurrencyApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBankAccountApi>(apiConfigs, refitSettings, httpClientHandler);

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

