using Microsoft.Extensions.DependencyInjection;

namespace PFM.CommonLibraries.Api.Monitoring;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestMetrics(this IServiceCollection services)
    {
        return services.AddSingleton<IRequestMetrics, RequestMetrics>();
    }
}