using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PFM.MovementAggregator.Monitoring.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTracing(this IServiceCollection services, TracingOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.MovementAggregator"))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder.AddSource(CustomActivitySource.Source.Name);
            
            if (options.Debug)
            {
                builder.AddConsoleExporter();
            }
        });

        return services;
    }
}