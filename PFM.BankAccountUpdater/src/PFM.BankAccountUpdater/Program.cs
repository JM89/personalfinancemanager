using PFM.BankAccountUpdater.Extensions;

namespace PFM.BankAccountUpdater
{
    public class Program
    {
        private static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((build, services) =>
                {
                    services
                        .AddMonitoring(build.Configuration, EnvironmentName)
                        .AddEventConsumerConfigurations(build.Configuration);

                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}