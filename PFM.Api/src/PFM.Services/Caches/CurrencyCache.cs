using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Currency;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches;

public interface ICurrencyCache
{
    Task<CurrencyDetails> GetById(int id);
}
    
public class CurrencyCache(IMemoryCache memoryCache, ICurrencyApi api) : ICurrencyCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));

    public async Task<CurrencyDetails> GetById(int id)
    {
        if (!memoryCache.TryGetValue(id, out CurrencyDetails value))
        {
            var apiResponse = await api.Get(id);
            value = JsonConvert.DeserializeObject<CurrencyDetails>(apiResponse.Data.ToString());
            memoryCache.Set(id, value, _options);
        }
        return value;
    }
}