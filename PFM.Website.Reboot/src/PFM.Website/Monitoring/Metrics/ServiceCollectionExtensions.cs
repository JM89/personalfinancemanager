using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace PFM.Website.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Website"))
            .WithMetrics(builder => builder.AddOtlpExporter());
        
        services.AddSingleton<IDashboardMetrics, DashboardMetrics>();
        
        services.ConfigureOpenTelemetryMeterProvider(builder =>
        {
            builder.AddMeter(DashboardMetrics.MeterName);

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