using Api.Extensions;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using PFM.CommonLibraries.Api.MiddleWares;
using PFM.Services.Core.Automapper;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

            builder.Services.AddControllers();

            builder.Services
                .AddAuthenticationAndAuthorization(builder.Configuration)
                .AddMonitoring(builder.Configuration, builder.Environment.EnvironmentName, out IMetricsRoot metrics)
                .AddEndpointsApiExplorer()
                .AddSwaggerDefinition()
                .AddEventPublisherConfigurations(builder.Configuration);

            builder.Services.AddDbContext<PFMContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("PFMConnection")));

            builder.Host
                .ConfigureMetrics(metrics)
                .UseMetrics(
                    options =>
                    {
                        options.EndpointOptions = endpointsOptions =>
                        {
                            endpointsOptions.MetricsTextEndpointOutputFormatter = metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
                            endpointsOptions.MetricsEndpointOutputFormatter = metrics.OutputMetricsFormatters.OfType<MetricsPrometheusProtobufOutputFormatter>().First();
                        };
                    });

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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMetricsAllEndpoints();

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