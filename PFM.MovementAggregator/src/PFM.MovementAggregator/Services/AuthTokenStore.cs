using PFM.MovementAggregator.Caches.Interfaces;
using PFM.MovementAggregator.Services.Interfaces;
using PFM.MovementAggregator.Settings;

namespace PFM.MovementAggregator.Services
{
    public class AuthTokenStore : IAuthTokenStore
    {
        private readonly AuthApiSettings _bankApiSettings;
        private readonly ITokenCache _tokenCache;

        public AuthTokenStore(AuthApiSettings bankApiSettings, ITokenCache tokenCache)
        {
            _bankApiSettings = bankApiSettings;
            _tokenCache = tokenCache;
        }

        public async Task<string> GetToken()
        {
            ArgumentException.ThrowIfNullOrEmpty(_bankApiSettings.ClientId);
            ArgumentException.ThrowIfNullOrEmpty(_bankApiSettings.ClientSecret);

            return await _tokenCache.GetToken(_bankApiSettings.ClientId, _bankApiSettings.ClientSecret);
        }
    }
}
