using System.Reflection;
using Api.Settings;
using EventStore.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Events;
using Services.Events.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Extensions
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
