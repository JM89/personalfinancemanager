using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace PFM.MovementAggregator.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.MovementAggregator"))
            .WithMetrics(builder => builder.AddOtlpExporter());
        
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