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
		public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
		{
			var authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>() ?? new AuthOptions();

            if (authOptions.DevOffline && !environment.IsDevelopment())
                throw new InvalidOperationException("Offline authentication is only available in the Development environment.");

            services.AddSingleton(authOptions);
            var cookieScheme = authOptions.DevOffline ? AuthOptions.OfflineCookieScheme : CookieAuthenticationDefaults.AuthenticationScheme;
            IdentityModelEventSource.ShowPII = true;

            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = cookieScheme;
                    opt.DefaultSignInScheme = cookieScheme;
                    opt.DefaultChallengeScheme = authOptions.DevOffline
                        ? cookieScheme
                        : OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(cookieScheme, options =>
                {
                    if (authOptions.DevOffline)
                    {
                        options.Cookie.Name = ".PFM.DevOffline";
                        options.LoginPath = "/login";
                        options.ReturnUrlParameter = "redirectUri";
                        options.Events.OnRedirectToLogin = context =>
                        {
                            if (context.Request.Path.StartsWithSegments("/api/frontend"))
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            else
                                context.Response.Redirect(context.RedirectUri);
                            return Task.CompletedTask;
                        };
                    }
                })
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
