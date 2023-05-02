using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Currency;
using PFM.Services.Caches.Interfaces;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches
{
    public class CurrencyCache : ICurrencyCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrencyApi _currencyApi;
        private readonly MemoryCacheEntryOptions _options;

        public CurrencyCache(IMemoryCache memoryCache, ICurrencyApi currencyApi)
        {
            this._memoryCache = memoryCache;
            this._currencyApi = currencyApi;
            this._options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));
        }

        public async Task<CurrencyDetails> GetById(int id)
        {
            if (!this._memoryCache.TryGetValue(id, out CurrencyDetails value))
            {
                var apiResponse = await _currencyApi.Get(id);
                value = JsonConvert.DeserializeObject<CurrencyDetails>(apiResponse.Data.ToString());
                _memoryCache.Set(id, value, _options);
            }
            return value;
        }
    }
}
