using OpenTelemetry.Metrics;
using PFM.Website.Configurations;

namespace PFM.Website.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static void ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
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
    }
}