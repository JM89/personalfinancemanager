using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFM.Api.Extensions;
using PFM.Api.Settings;
using PFM.CommonLibraries.Api.MiddleWares;
using PFM.DataAccessLayer;
using PFM.Services.Caches;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Core.Automapper;
using PFM.Services.Monitoring.Logging;
using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;
using PFM.Services.MovementStrategy;

namespace PFM.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

            if (builder.Environment.EnvironmentName != "Production")
            {
                Console.WriteLine(builder.Configuration.GetDebugView());
            }

            var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();
            builder.Services.AddSingleton(appSettings);
            
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<AuthHeaderHandler>();

            builder.Services.AddControllers();

            builder.Services.AddMemoryCache();

            builder.Services
                .AddSingleton<ContextMovementStrategy>()
                .AddSingleton<IExpenseTypeCache, ExpenseTypeCache>();

            builder.Services
                .AddAuthenticationAndAuthorization(builder.Configuration)
                .AddBankApi(builder.Configuration, builder.Environment.EnvironmentName != "Production")
                .ConfigureLogging(builder.Configuration, builder.Environment.EnvironmentName)
                .ConfigureTracing(appSettings.TracingOptions)
                .ConfigureMetrics(appSettings.MetricsOptions)
                .AddEndpointsApiExplorer()
                .AddSwaggerDefinition()
                .AddEventPublisherConfigurations(builder.Configuration);

            builder.Services.AddDbContext<PFMContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("PFMConnection")));

            var dataSettings = builder.Configuration.GetSection("DataSettings").Get<DataSettings>() ?? new DataSettings();
            builder.Services.AddSingleton(dataSettings);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new AutoFacModule()));

            var app = builder.Build();

            app.UseMiddleware<TimedOperationMiddleware>();
            app.UseMiddleware<PFM.Api.Middlewares.ResponseWrapperMiddleware>();

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
                cfg.AddProfile<EntityToEntityMapping>();
                cfg.AddProfile<SearchParametersMapping>();
            });

            app.Run();
        }
    }
}