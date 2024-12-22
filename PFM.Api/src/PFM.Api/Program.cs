using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFM.Api.Configurations.Monitoring.Logging;
using PFM.Api.Configurations.Monitoring.Metrics;
using PFM.Api.Configurations.Monitoring.Tracing;
using PFM.Api.Extensions;
using PFM.Api.Middlewares;
using PFM.Api.MiddleWares;
using PFM.Api.Settings;
using PFM.DataAccessLayer;
using PFM.Services.Caches;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Core.Automapper;
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
                cfg.AddProfile<EntityToEntityMapping>();
                cfg.AddProfile<SearchParametersMapping>();
            });

            app.Run();
        }
    }
}