using PFM.BankAccountUpdater.Caches.Interfaces;
using PFM.BankAccountUpdater.Services.Interfaces;
using PFM.BankAccountUpdater.Settings;

namespace PFM.BankAccountUpdater.Services
{
    public class AuthTokenStore(AuthApiSettings bankApiSettings, ITokenCache tokenCache) : IAuthTokenStore
    {
        public async Task<string> GetToken()
        {
            ArgumentException.ThrowIfNullOrEmpty(bankApiSettings.ClientId);
            ArgumentException.ThrowIfNullOrEmpty(bankApiSettings.ClientSecret);

            return await tokenCache.GetToken(bankApiSettings.ClientId, bankApiSettings.ClientSecret);
        }
    }
}
