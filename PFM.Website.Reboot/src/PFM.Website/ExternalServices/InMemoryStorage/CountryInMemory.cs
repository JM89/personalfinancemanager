using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Country;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class CountryInMemory : ICountryApi
    {
        internal readonly IList<CountryDetails> Storage = new List<CountryDetails>() {
            new () { Id = 1, Name = "United Kingdom" },
            new () { Id = 2, Name = "France" }
        };

        public async Task<ApiResponse> Get()
        {
            var result = JsonConvert.SerializeObject(Storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(Storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }
    }
}

