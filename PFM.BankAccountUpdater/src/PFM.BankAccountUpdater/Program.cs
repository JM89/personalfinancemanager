using PFM.BankAccountUpdater.Extensions;

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
                .ConfigureServices((build, services) =>
                {
                    services
                        .AddMemoryCache()
                        .AddServices()
                        .AddMonitoring(build.Configuration, EnvironmentName)
                        .AddBankApi(build.Configuration, EnvironmentName != "Production")
                        .AddAuthenticationAndAuthorization(build.Configuration)
                        .AddEventHandlers()
                        .AddEventConsumerConfigurations(build.Configuration);

                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}