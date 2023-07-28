﻿using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PFM.Website.ExternalServices;
using PFM.Website.ExternalServices.InMemoryStorage;
using Refit;

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



        public static IServiceCollection AddPfmApi(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnvironment)
        {
            var useApi = configuration.GetValue<bool>("UsePfmApi");

            if (!useApi)
            {
                services.AddSingleton<IExpenseTypeApi, ExpenseTypeInMemory>();
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
                .AddRefitClient<ExternalServices.IExpenseTypeApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfigs))
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
                .AddHttpMessageHandler<AuthHeaderHandler>();

            return services;
        }
    }
}
