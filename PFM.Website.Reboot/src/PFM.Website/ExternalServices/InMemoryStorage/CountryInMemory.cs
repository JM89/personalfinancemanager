using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Country;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class CountryInMemory : ICountryApi
    {
        internal IList<CountryDetails> _storage;
        
        public CountryInMemory()
        {
            _storage = new List<CountryDetails>() {
                new CountryDetails() { Id = 1, Name = "United Kingdom" },
                new CountryDetails() { Id = 2, Name = "France" }
            };
        }

        public async Task<ApiResponse> Get()
        {
            var result = JsonConvert.SerializeObject(_storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }
    }
}

