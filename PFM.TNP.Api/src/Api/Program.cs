using Api.Configurations.Monitoring.Logging;
using Api.Configurations.Monitoring.Metrics;
using Api.Configurations.Monitoring.Tracing;
using Api.Extensions;
using Api.Middlewares;
using Api.MiddleWares;
using Api.Settings;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using PFM.Services.Core.Automapper;
using Services;
using Services.Interfaces;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

            builder.Services.AddControllers();

            var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();
            
            builder.Services
                .AddAuthenticationAndAuthorization(builder.Configuration)
                .ConfigureLogging(builder.Configuration, builder.Environment.EnvironmentName)
                .ConfigureTracing(appSettings.TracingOptions)
                .ConfigureMetrics(appSettings.MetricsOptions)
                .AddEndpointsApiExplorer()
                .AddSwaggerDefinition();

            builder.Services.AddDbContext<PFMContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("PFMConnection")));

            builder.Services.AddTransient<IBankRepository, BankRepository>();
            builder.Services.AddTransient<IBankService, BankService>();
            
            var app = builder.Build();

            app.UseMiddleware<TimedOperationMiddleware>();
            app.UseMiddleware<ResponseWrapperMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
            });

            app.Run();
        }
    }
}