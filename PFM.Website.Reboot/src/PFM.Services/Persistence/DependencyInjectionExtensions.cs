using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using PFM.Services.Configurations;
using PFM.Services.Persistence.Implementations;

namespace PFM.Services.Persistence;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddObjectStorage(this IServiceCollection services, ExternalServiceSettings settings)
    {
        if (!settings.BankIconSettings.UseRemoteStorage)
        {
            services.AddSingleton<IObjectStorageService, LocalStorageService>();
            return services;
        }
        
        var s3Config = new AmazonS3Config() {
            RegionEndpoint = RegionEndpoint.GetBySystemName(settings.AwsRegion)
        };

        if (!string.IsNullOrEmpty(settings.AwsEndpointUrl))
        {
            s3Config.ServiceURL = settings.AwsEndpointUrl;
            s3Config.AuthenticationRegion = settings.AwsRegion;
            s3Config.ForcePathStyle = true;
        }

        services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Config));

        services.AddSingleton<IObjectStorageService, AwsS3Service>();
        
        return services;
    }
}