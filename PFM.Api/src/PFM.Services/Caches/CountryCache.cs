using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Country;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches;

public interface ICountryCache
{
    Task<CountryDetails> GetById(int id);
}

public class CountryCache(IMemoryCache memoryCache, ICountryApi api) : ICountryCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));

    public async Task<CountryDetails> GetById(int id)
    {
        if (!memoryCache.TryGetValue(id, out CountryDetails value))
        {
            var apiResponse = await api.Get(id);
            value = JsonConvert.DeserializeObject<CountryDetails>(apiResponse.Data.ToString());
            memoryCache.Set(id, value, _options);
        }
        return value;
    }
}