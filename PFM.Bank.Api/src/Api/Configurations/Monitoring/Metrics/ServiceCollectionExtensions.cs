using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Api.Configurations.Monitoring.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMetrics(this IServiceCollection services, MetricsOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Bank.Api"))
            .WithMetrics(builder => builder.AddOtlpExporter());
        
        services.AddSingleton<IRequestMetrics, RequestMetrics>();
        
        services.ConfigureOpenTelemetryMeterProvider(builder =>
        {
            builder.AddMeter(RequestMetrics.MeterName);
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