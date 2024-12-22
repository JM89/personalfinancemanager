using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using PFM.CommonLibraries.Api.Monitoring;

namespace Services.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Bank.Api"))
            .WithMetrics(builder => builder.AddOtlpExporter());
        
        services.AddRequestMetrics();
        
        services.ConfigureOpenTelemetryMeterProvider(builder =>
        {
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