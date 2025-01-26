using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PFM.Services.Monitoring.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTracing(this IServiceCollection services, TracingOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: options.ServiceName))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder.AddAspNetCoreInstrumentation(x => 
                x.Filter = context => Filter(context.Request.Path));
            
            if (options.Debug)
            {
                builder.AddConsoleExporter();
            }
        });

        return services;
    }
    
    private static readonly string[] FilterPaths = new[]
    {
        "/_blazor/initializers",
        "/_blazor/negotiate",
        "/_blazor/disconnect",
        "/css"
    };

    private static bool Filter(string? path)
    {
        return path == null || !FilterPaths.Any(x => x.StartsWith(path));
    }
}