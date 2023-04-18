using Serilog;

namespace PFM.Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Currently set Logging only.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonitoring(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddSingleton(Log.Logger);
            return services;
        }
    }
}
