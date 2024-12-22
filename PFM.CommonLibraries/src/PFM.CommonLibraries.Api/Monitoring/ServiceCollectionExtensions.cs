using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace PFM.CommonLibraries.Api.Monitoring;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestMetrics(this IServiceCollection services)
    {
        services.AddSingleton<IRequestMetrics, RequestMetrics>();

        services.ConfigureOpenTelemetryMeterProvider(builder =>
        {
            builder.AddMeter(RequestMetrics.MeterName);
        });
        
        return services;
    }
}