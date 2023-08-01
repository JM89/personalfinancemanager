using Api.Contracts.Shared;
using AutoFixture;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Currency;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class CurrencyInMemory : ICurrencyApi
    {
        internal IList<CurrencyDetails> _storage;
        
        public CurrencyInMemory()
        {
            _storage = new List<CurrencyDetails>()
            {
                new CurrencyDetails() { Id = 1, Name = "GBP", Symbol = "£" },
                new CurrencyDetails() { Id = 2, Name = "EUR", Symbol = "€" },
                new CurrencyDetails() { Id = 3, Name = "USD", Symbol = "$" }
            };
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> GetList()
        {
            var result = JsonConvert.SerializeObject(_storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }
    }
}

