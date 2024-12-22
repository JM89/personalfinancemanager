using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PFM.Api.Configurations.Monitoring.Tracing;

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
            builder
                .AddHttpClientInstrumentation(opt =>
                {
                    opt.EnrichWithHttpRequestMessage = (activity, message) =>
                    {
                        if (message?.RequestUri?.AbsolutePath != "/event_store.client.streams.Streams/Append") 
                            return;
                        
                        EnrichWithEventStorePublishTags(activity);
                    };
                })
                .AddEntityFrameworkCoreInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.EnrichWithIDbCommand = (activity, command) =>
                    {
                        var querySummary = Activity.Current?.GetBaggageItem("db.query.summary") ??
                                           command.CommandType.ToString();
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

    private const string EventStoreSystem = "eventstore";

    private static void EnrichWithEventStorePublishTags(Activity activity)
    {
        activity?.AddTag("peer.service", EventStoreSystem);
        activity?.AddTag("messaging.operation.type", "send");
        activity?.AddTag("messaging.system", EventStoreSystem);
        activity?.AddTag("db.operation.name", "APPEND");
        activity?.AddTag("db.system", EventStoreSystem);
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