using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace PFM.BankAccountUpdater.Configurations.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.BankAccountUpdater"))
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