using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.Services.Caches.Interfaces;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches
{
    public class BankAccountCache : IBankAccountCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IBankAccountApi _bankAccountApi;
        private readonly MemoryCacheEntryOptions _options;

        public BankAccountCache(IMemoryCache memoryCache, IBankAccountApi bankAccountApi)
        {
            this._memoryCache = memoryCache;
            this._bankAccountApi = bankAccountApi;
            this._options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
        }

        public async Task<AccountDetails> GetById(int id)
        {
            if (!this._memoryCache.TryGetValue(id, out AccountDetails value))
            {
                var apiResponse = await _bankAccountApi.Get(id);
                value = JsonConvert.DeserializeObject<AccountDetails>(apiResponse.Data.ToString());
                _memoryCache.Set(id, value, _options);
            }

            return value;
        }
    }
}
