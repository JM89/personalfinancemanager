using Microsoft.Extensions.Caching.Memory;
using PFM.MovementAggregator.Caches.Interfaces;
using PFM.MovementAggregator.ExternalServices.AuthApi;
using PFM.MovementAggregator.ExternalServices.AuthApi.Contracts;

namespace PFM.MovementAggregator.Caches
{
    public class TokenCache: ITokenCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAuthApi _authApi;
        private readonly MemoryCacheEntryOptions _options;

        public TokenCache(IMemoryCache memoryCache, IAuthApi authApi)
        {
            this._memoryCache = memoryCache;
            this._authApi = authApi;
            this._options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));
        }

        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            if (!this._memoryCache.TryGetValue(clientId, out string? token))
            {
                var response = await _authApi.Login(new ClientInfo(clientId, clientSecret));
                token = response.AccessToken;
                _memoryCache.Set(clientId, token, _options);
            }
            return token ?? "";
        }
    }
}
