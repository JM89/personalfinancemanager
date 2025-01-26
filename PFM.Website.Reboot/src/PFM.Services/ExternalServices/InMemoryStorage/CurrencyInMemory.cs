using Newtonsoft.Json;
using PFM.Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Currency;

namespace PFM.Services.ExternalServices.InMemoryStorage
{
    public class CurrencyInMemory : ICurrencyApi
    {
        internal readonly IList<CurrencyDetails> Storage = new List<CurrencyDetails>()
        {
            new() { Id = 1, Name = "GBP", Symbol = "£" },
            new() { Id = 2, Name = "EUR", Symbol = "€" },
            new() { Id = 3, Name = "USD", Symbol = "$" }
        };

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(Storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> GetList()
        {
            var result = JsonConvert.SerializeObject(Storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }
    }
}

