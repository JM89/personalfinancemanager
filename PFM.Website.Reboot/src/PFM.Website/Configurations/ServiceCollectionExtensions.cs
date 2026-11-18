using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace PFM.Website.Configurations
{
	public static class ServiceCollectionExtensions
	{
        /// <summary>
        /// Registers ASP.NET Core's Problem Details JSON formatter. Does not install exception-handling middleware.
        /// </summary>
        public static IServiceCollection AddJsonErrorResponses(this IServiceCollection services) =>
            services.AddProblemDetails();

        /// <summary>
        /// Configures website session cookies and live Keycloak sign-in. Unauthenticated
        /// frontend API requests receive 401; only the login page starts the live OIDC flow.
        /// </summary>
		public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
		{
			var authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>() ?? new AuthOptions();

            if (authOptions.DevOffline && !environment.IsDevelopment())
                throw new InvalidOperationException("Offline authentication is only available in the Development environment.");

            services.AddSingleton(authOptions);
            var cookieScheme = authOptions.DevOffline ? AuthOptions.OfflineCookieScheme : CookieAuthenticationDefaults.AuthenticationScheme;
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = cookieScheme;
                    opt.DefaultSignInScheme = cookieScheme;
                    // Return 401 to React API calls; /login explicitly starts OIDC sign-in in live mode.
                    opt.DefaultChallengeScheme = cookieScheme;
                })
                .AddCookie(cookieScheme, options =>
                {
                    if (authOptions.DevOffline)
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
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = authOptions.Authority;

                    options.RequireHttpsMetadata = !environment.IsDevelopment();

                    options.ClientId = authOptions.ClientId;
                    options.ClientSecret = authOptions.ClientSecret;
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    // In split-host mode Keycloak rejects the public-issuer token at the internal UserInfo URL.
                    options.GetClaimsFromUserInfoEndpoint = string.IsNullOrWhiteSpace(authOptions.PublicAuthority);
                    options.UseTokenLifetime = false;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.ClaimActions.MapUniqueJsonKey("preferred_username", "preferred_username");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.PublicAuthority
                    };

                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = context =>
                        {
                            if (!string.IsNullOrWhiteSpace(authOptions.PublicAuthority) &&
                                context.ProtocolMessage.IssuerAddress.StartsWith(authOptions.Authority.TrimEnd('/') + "/", StringComparison.Ordinal))
                                context.ProtocolMessage.IssuerAddress = authOptions.PublicAuthority.TrimEnd('/') +
                                    context.ProtocolMessage.IssuerAddress[authOptions.Authority.TrimEnd('/').Length..];
                            return Task.CompletedTask;
                        },
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
