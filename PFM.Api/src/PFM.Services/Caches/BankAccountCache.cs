using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.Services.ExternalServices.BankApi;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches;

public interface IBankAccountCache
{
    Task<AccountDetails> GetById(int id);
}

public class BankAccountCache(IMemoryCache memoryCache, IBankAccountApi api) : IBankAccountCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

    public async Task<AccountDetails> GetById(int id)
    {
        if (!memoryCache.TryGetValue(id, out AccountDetails value))
        {
            var apiResponse = await api.Get(id);
            value = JsonConvert.DeserializeObject<AccountDetails>(apiResponse.Data.ToString());
            memoryCache.Set(id, value, _options);
        }

        return value;
    }
}