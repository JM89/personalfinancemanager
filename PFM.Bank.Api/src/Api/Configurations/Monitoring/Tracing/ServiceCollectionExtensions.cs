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
            .ConfigureResource(builder => builder.AddService(serviceName: "PFM.Bank.Api"))
            .WithTracing(builder => builder.AddOtlpExporter());
        
        services.ConfigureOpenTelemetryTracerProvider(builder =>
        {
            builder
                .AddEntityFrameworkCoreInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.EnrichWithIDbCommand = (activity, command) =>
                    {
                        var querySummary = Activity.Current?.GetBaggageItem("db.query.summary") ?? command.CommandType.ToString();
                        var operation = Activity.Current?.GetBaggageItem("db.operation.name") ?? "UNKNOWN";

                        activity.SetTag("db.name", activity.DisplayName);
                        activity.SetTag("db.query.summary", querySummary);
                        activity.SetTag("db.operation", operation);
                        activity.DisplayName = $"Run {querySummary}";
                    };
                })
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