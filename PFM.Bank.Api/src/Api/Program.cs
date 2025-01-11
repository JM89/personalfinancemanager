using System.Reflection;
using Api.Configurations.Monitoring.Logging;
using Api.Configurations.Monitoring.Metrics;
using Api.Configurations.Monitoring.Tracing;
using Api.Extensions;
using Api.Middlewares;
using Api.MiddleWares;
using Api.Settings;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using PFM.Services.Core.Automapper;

namespace Api
{
    public static class Program
    {
        internal static readonly string AssemblyVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0.1";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

            builder.Services.AddControllers();

            var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();
            
            builder.Services
                .AddAuthenticationAndAuthorization(appSettings.AuthOptions)
                .ConfigureLogging(builder.Configuration, builder.Environment.EnvironmentName)
                .ConfigureTracing(appSettings.TracingOptions)
                .ConfigureMetrics(appSettings.MetricsOptions)
                .AddEndpointsApiExplorer()
                .AddSwaggerDefinition(appSettings)
                .AddEventPublisherConfigurations(builder.Configuration);

            builder.Services.AddDbContext<PFMContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("PFMConnection")));

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutoFacModule()));

            var app = builder.Build();

            app.UseMiddleware<TimedOperationMiddleware>();
            app.UseMiddleware<ResponseWrapperMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            if (appSettings.AuthOptions.Enabled)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.MapControllers();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
                cfg.AddProfile<EntityToContractMapping>();
            });

            app.Run();
        }
    }
}