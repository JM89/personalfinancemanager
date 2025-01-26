using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Refit;
using AutoMapper;
using Microsoft.IdentityModel.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PFM.Services.ExternalServices;
using PFM.Services.ExternalServices.InMemoryStorage;
using PFM.Services.Mappers;

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
    }
}

