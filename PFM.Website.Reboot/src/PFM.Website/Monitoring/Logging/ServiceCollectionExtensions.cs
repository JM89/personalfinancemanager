using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;

namespace PFM.Website.Monitoring.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        return services;
    }
}