using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PFM.BankAccountUpdater.Configurations.Monitoring.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTracing(this IServiceCollection services, TracingOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.BankAccountUpdater"))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder.AddSource(CustomActivitySource.Source.Name);
            builder.AddHttpClientInstrumentation();
            
            if (options.Debug)
            {
                builder.AddConsoleExporter();
            }
        });

        return services;
    }
}