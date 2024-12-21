using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace PFM.Services.Monitoring.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment)
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        return services;
    }
}