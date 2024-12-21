using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using PFM.CommonLibraries.Api.Monitoring;

namespace PFM.Services.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Website"))
            .WithMetrics(builder => builder.AddOtlpExporter());
        
        services.AddSingleton<IExampleMetrics, ExampleMetrics>();
        services.AddRequestMetrics();
        
        services.ConfigureOpenTelemetryMeterProvider(builder =>
        {
            builder.AddMeter(ExampleMetrics.MeterName);

            if (!options.Debug)
            {
                builder.AddRuntimeInstrumentation();
            }
            else
            {
                builder.AddConsoleExporter();
            }
        });

        return services;
    }
}