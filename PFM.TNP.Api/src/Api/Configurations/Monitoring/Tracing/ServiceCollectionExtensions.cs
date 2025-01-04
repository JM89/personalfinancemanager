using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Api.Configurations.Monitoring.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTracing(this IServiceCollection services, TracingOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.TNP.Api"))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder
                .AddSource("MySqlConnector")
                .AddAspNetCoreInstrumentation(x => 
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
        "/swagger"
    };

    private static bool Filter(string path)
    {
        return path == null || !FilterPaths.Any(x => x.StartsWith(path));
    }
}