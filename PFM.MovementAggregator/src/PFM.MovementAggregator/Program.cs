using PFM.MovementAggregator.Extensions;
using PFM.MovementAggregator.Monitoring.Logging;
using PFM.MovementAggregator.Monitoring.Metrics;
using PFM.MovementAggregator.Monitoring.Tracing;
using PFM.MovementAggregator.Settings;

namespace PFM.MovementAggregator
{
    public class Program
    {
        private static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c => {
                    c.AddEnvironmentVariables(prefix: "APP_");
                })
                .ConfigureServices((builder, services) =>
                {
                    var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();

                    services
                        .AddMemoryCache()
                        .AddServices(appSettings)
                        .ConfigureLogging(builder.Configuration, EnvironmentName)
                        .ConfigureTracing(appSettings.TracingOptions)
                        .ConfigureMetrics(appSettings.MetricsOptions)    
                        .AddAuthenticationAndAuthorization(builder.Configuration)
                        .AddEventHandlers()
                        .AddEventConsumerConfigurations(builder.Configuration);

                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}