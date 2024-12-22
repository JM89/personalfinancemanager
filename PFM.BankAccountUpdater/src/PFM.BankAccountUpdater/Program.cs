using PFM.BankAccountUpdater.Configurations.Monitoring.Logging;
using PFM.BankAccountUpdater.Configurations.Monitoring.Metrics;
using PFM.BankAccountUpdater.Configurations.Monitoring.Tracing;
using PFM.BankAccountUpdater.Extensions;
using PFM.BankAccountUpdater.Settings;

namespace PFM.BankAccountUpdater
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
                        .AddServices()
                        .ConfigureLogging(builder.Configuration, EnvironmentName)
                        .ConfigureTracing(appSettings.TracingOptions)
                        .ConfigureMetrics(appSettings.MetricsOptions)                        
                        .AddBankApi(builder.Configuration, EnvironmentName != "Production")
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