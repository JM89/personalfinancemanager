using OpenTelemetry.Metrics;
using PFM.Services.Monitoring.Metrics;

namespace PFM.Website.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureWebsiteMetrics(this IServiceCollection services, MetricsOptions options)
    { 
        services.AddSingleton<IDashboardMetrics, DashboardMetrics>();
        services.ConfigureMetrics(options);
        
        services.ConfigureOpenTelemetryMeterProvider(builder =>
        {
            builder.AddMeter(DashboardMetrics.MeterName);
        });

        return services;
    }
}