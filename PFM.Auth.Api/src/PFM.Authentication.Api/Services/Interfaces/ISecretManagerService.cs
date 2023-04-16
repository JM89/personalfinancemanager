using System;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Services.Interfaces
{
    public interface ISecretManagerService
    {
        Task<bool> CreateApiKeySecrets(Guid key, string appName, string value);

        Task<string> GetApiKeySecrets(Guid key);

        Task<T> GetSecrets<T>(string secretName);
    }
}
