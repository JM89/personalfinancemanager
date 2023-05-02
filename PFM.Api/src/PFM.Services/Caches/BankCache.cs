using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.Caches.Interfaces;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches
{
    public class BankCache : IBankCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IBankApi _bankApi;
        private readonly MemoryCacheEntryOptions _options;

        public BankCache(IMemoryCache memoryCache, IBankApi bankApi)
        {
            this._memoryCache = memoryCache;
            this._bankApi = bankApi;
            this._options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));
        }

        public async Task<BankDetails> GetById(int id)
        {
            if (!this._memoryCache.TryGetValue(id, out BankDetails value))
            {
                var apiResponse = await _bankApi.Get(id);
                value = JsonConvert.DeserializeObject<BankDetails>(apiResponse.Data.ToString());
                _memoryCache.Set(id, value, _options);
            }
            return value;
        }
    }
}
