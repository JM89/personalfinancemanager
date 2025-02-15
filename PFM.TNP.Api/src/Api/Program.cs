using System.Reflection;
using Api.Configurations.Monitoring.Logging;
using Api.Configurations.Monitoring.Metrics;
using Api.Configurations.Monitoring.Tracing;
using Api.Extensions;
using Api.Middlewares;
using Api.MiddleWares;
using Api.Settings;
using AutoMapper;
using Dapper;
using DataAccessLayer.Mapping;
using DataAccessLayer.Repositories;
using Services;
using Services.Core.Automapper;

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
                .AddSwaggerDefinition(appSettings);

            // Data Access Layer
            
            builder.Services.AddSingleton(appSettings.DatabaseOptions);
            builder.Services.AddTransient<IPensionRepository, PensionRepository>();
            builder.Services.AddTransient<IIncomeTaxReportRepository, IncomeTaxReportRepository>();
            
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            
            // Service Layer
            
            builder.Services.AddTransient<IPensionService, PensionService>();
            builder.Services.AddTransient<IIncomeTaxReportService, IncomeTaxReportService>();

            builder.Services.AddAutoMapper(typeof(ModelToEntityMapping).Assembly);
                
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

            app.Run();
        }
    }
}