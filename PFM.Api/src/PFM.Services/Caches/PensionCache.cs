using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.Pension;

namespace PFM.Services.Caches;

public interface IPensionCache
{
    Task<PensionDetails> GetById(Guid id);
}

public class PensionCache(IMemoryCache memoryCache, IPensionApi api) : IPensionCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));

    public async Task<PensionDetails> GetById(Guid id)
    {
        if (!memoryCache.TryGetValue(id, out PensionDetails value))
        {
            var apiResponse = await api.Get(id);
            value = JsonConvert.DeserializeObject<PensionDetails>(apiResponse.Data.ToString());
            memoryCache.Set(id, value, _options);
        }
        return value;
    }
}