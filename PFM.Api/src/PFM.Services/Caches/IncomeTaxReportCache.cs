using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.IncomeTaxReport;

namespace PFM.Services.Caches;

public interface IIncomeTaxReportCache
{
    Task<IncomeTaxReportDetails> GetById(Guid id);
}
    
public class IncomeTaxReportCache(IMemoryCache memoryCache, IIncomeTaxReportApi api) : IIncomeTaxReportCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));

    public async Task<IncomeTaxReportDetails> GetById(Guid id)
    {
        if (!memoryCache.TryGetValue(id, out IncomeTaxReportDetails value))
        {
            var apiResponse = await api.Get(id);
            value = JsonConvert.DeserializeObject<IncomeTaxReportDetails>(apiResponse.Data.ToString());
            memoryCache.Set(id, value, _options);
        }
        return value;
    }
}