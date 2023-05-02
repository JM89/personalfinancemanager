using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Country;
using PFM.Services.Caches.Interfaces;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches
{
    public class CountryCache : ICountryCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICountryApi _countryApi;
        private readonly MemoryCacheEntryOptions _options;

        public CountryCache(IMemoryCache memoryCache, ICountryApi countryApi)
        {
            this._memoryCache = memoryCache;
            this._countryApi = countryApi;
            this._options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4)); 
        }

        public async Task<CountryDetails> GetById(int id)
        {
            if (!this._memoryCache.TryGetValue(id, out CountryDetails value))
            {
                var apiResponse = await _countryApi.Get(id);
                value = JsonConvert.DeserializeObject<CountryDetails>(apiResponse.Data.ToString());
                _memoryCache.Set(id, value, _options);
            }
            return value;
        }
    }
}
