using Microsoft.Extensions.Caching.Memory;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Caches
{
    public class ExpenseTypeCache : IExpenseTypeCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly MemoryCacheEntryOptions _options;

        public ExpenseTypeCache(IMemoryCache memoryCache, IExpenseTypeRepository expenseTypeRepository)
        {
            this._memoryCache = memoryCache;
            this._expenseTypeRepository = expenseTypeRepository;
            this._options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));
        }

        public Task<ExpenseType> GetById(int id)
        {
            if (!this._memoryCache.TryGetValue(id, out ExpenseType value))
            {
                var response = _expenseTypeRepository.GetById(id);
                _memoryCache.Set(id, response, _options);
            }
            return Task.FromResult(value);
        }
    }
}
