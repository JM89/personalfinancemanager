﻿using Microsoft.Extensions.Caching.Memory;
using PFM.BankAccountUpdater.Caches.Interfaces;
using PFM.BankAccountUpdater.ExternalServices.AuthApi;

namespace PFM.BankAccountUpdater.Caches
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
                var response = await _authApi.Login(new Authentication.Api.DTOs.UserRequest()
                {
                    Username = clientId, 
                    Password = clientSecret
                });
                token = response.Token;
                _memoryCache.Set(clientId, token, _options);
            }
            return token ?? "";
        }
    }
}