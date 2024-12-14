using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PFM.Website.Monitoring.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTracing(this IServiceCollection services, TracingOptions options)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Website"))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder.AddAspNetCoreInstrumentation(x => 
                x.Filter = context => Filter(context.Request.PathBase));
            
            builder.AddSource(WebActivitySource.Source.Name);

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
        return true;
        //return path == null || !FilterPaths.Any(x => x.StartsWith(path));
    }
}