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
            _storage = Enumerable.Range(1, 5).Select(index => new CountryDetails
            {
                Id = index,
                Name = $"Country #{index}"
            }).ToList();
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

