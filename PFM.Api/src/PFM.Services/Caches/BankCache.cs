using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches;

public interface IBankCache
{
    Task<BankDetails> GetById(int id);
}
    
public class BankCache(IMemoryCache memoryCache, IBankApi api) : IBankCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));

    public async Task<BankDetails> GetById(int id)
    {
        if (!memoryCache.TryGetValue(id, out BankDetails value))
        {
            var apiResponse = await api.Get(id);
            value = JsonConvert.DeserializeObject<BankDetails>(apiResponse.Data.ToString());
            memoryCache.Set(id, value, _options);
        }
        return value;
    }
}
