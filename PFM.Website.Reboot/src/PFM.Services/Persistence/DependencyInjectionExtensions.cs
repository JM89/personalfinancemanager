using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using PFM.Services.Persistence.Implementations;

namespace PFM.Services.Persistence;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
        return services.AddSingleton<IObjectStorageService, LocalStorageService>();
    }
    
    public static IServiceCollection AddRemoteStorage(this IServiceCollection services, string awsEndpointUrl, string awsRegion)
    {
        var s3Config = new AmazonS3Config() {
            RegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion)
        };

        if (!string.IsNullOrEmpty(awsEndpointUrl))
        {
            s3Config.ServiceURL = awsEndpointUrl;
            s3Config.AuthenticationRegion = awsRegion;
            s3Config.ForcePathStyle = true;
        }

        services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Config));

        services.AddSingleton<IObjectStorageService, AwsS3Service>();
        
        return services;
    }
}