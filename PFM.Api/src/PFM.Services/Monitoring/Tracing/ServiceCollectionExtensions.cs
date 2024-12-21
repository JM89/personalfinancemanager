using System.Linq;
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
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Api"))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder.AddAspNetCoreInstrumentation(x => 
                x.Filter = context => Filter(context.Request.PathBase));
            
            builder.AddSource(ApiActivitySource.Source.Name);

            if (options.Debug)
            {
                builder.AddConsoleExporter();
            }
        });

        return services;
    }
    
    private static readonly string[] FilterPaths = new[]
    {
        "/swagger"
    };

    private static bool Filter(string path)
    {
        return path == null || !FilterPaths.Any(x => x.StartsWith(path));
    }
}