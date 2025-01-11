using Microsoft.Extensions.Caching.Memory;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches;

public interface IExpenseTypeCache
{
    Task<string> GetById(int id);
}
    
public class ExpenseTypeCache(IMemoryCache memoryCache, IExpenseTypeRepository repository)
    : IExpenseTypeCache
{
    private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));

    public Task<string> GetById(int id)
    {
        if (!memoryCache.TryGetValue(id, out string value))
        {
            var response = repository.GetById(id);
            value = response?.Name ?? "Unknown";
            memoryCache.Set(id, value, _options);
        }
        return Task.FromResult(value);
    }
}