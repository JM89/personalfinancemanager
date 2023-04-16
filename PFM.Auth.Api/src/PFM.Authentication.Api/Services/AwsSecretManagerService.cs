using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using PFM.Authentication.Api.Helpers;
using PFM.Authentication.Api.Models;
using PFM.Authentication.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Services
{
    public class AwsSecretManagerService : ISecretManagerService
    {
        private const string ApiKeyPrefix = "pfm/apikeys/";

        private readonly AmazonSecretsManagerClient _client;

        public AwsSecretManagerService(AppSettings appSettings)
        {
            var config = new AmazonSecretsManagerConfig();
            if (!string.IsNullOrEmpty(appSettings.AwsEndpointUrl))
            {
                config.ServiceURL = appSettings.AwsEndpointUrl;
                config.AuthenticationRegion = appSettings.AwsRegion;
            }
            _client = new AmazonSecretsManagerClient(config);
        }

        public async Task<bool> CreateApiKeySecrets(Guid key, string appName, string apiKey)
        {
            var secretRequest = new CreateSecretRequest
            {
                Name = $"{ApiKeyPrefix}{key}",
                SecretString = JsonConvert.SerializeObject(new SecretApiKeyModel()
                {
                    AppName = appName,
                    ApiKey = apiKey
                }),
                Description = $"Secret API Key for {appName}"
            };
            try
            {
                var response = await _client.CreateSecretAsync(secretRequest);
                return response?.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (ResourceExistsException)
            {
                return false;
            }
        }

        public async Task<string> GetApiKeySecrets(Guid key)
        {
            var request = new GetSecretValueRequest { SecretId = $"{ApiKeyPrefix}{key}" };
            try 
            {
                var response = await _client.GetSecretValueAsync(request);
                if (!string.IsNullOrEmpty(response?.SecretString))
                {
                    var secret = JsonConvert.DeserializeObject<SecretApiKeyModel>(response?.SecretString);
                    return secret.ApiKey;
                }
                return "";
            } 
            catch (ResourceNotFoundException)
            {
                return null;
            }
        }

        public async Task<T> GetSecrets<T>(string secretName)
        {
            var request = new GetSecretValueRequest { SecretId = secretName };
            try
            {
                var response = await _client.GetSecretValueAsync(request);
                if (!string.IsNullOrEmpty(response?.SecretString))
                {
                    var secret = JsonConvert.DeserializeObject<T>(response?.SecretString);
                    return secret;
                }
                return default(T);
            }
            catch (ResourceNotFoundException)
            {
                return default(T);
            }
        }
    }
}
