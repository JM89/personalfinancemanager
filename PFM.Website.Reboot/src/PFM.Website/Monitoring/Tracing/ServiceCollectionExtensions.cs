using OpenTelemetry.Trace;
using PFM.Services.Monitoring.Tracing;

namespace PFM.Website.Monitoring.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureWebsiteTracing(this IServiceCollection services, TracingOptions options)
    {
        services.ConfigureTracing(options);
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder.AddSource(WebActivitySource.Source.Name);
        });

        return services;
    }
}