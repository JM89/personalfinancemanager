using Api.Contracts.Shared;
using AutoFixture;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Bank;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class BankInMemory : IBankApi
    {
        internal IList<BankDetails> _storage;
        
        public BankInMemory()
        {
            var countries = new CountryInMemory()._storage.ToList();
            var fixture = new Fixture();

            var rng = new Random();
            _storage = new List<BankDetails>();
            for (int i = 0; i <= 5; i++) {
                var item = fixture.Build<BankDetails>()
                    .With(x => x.Id, i)
                    .With(x => x.CountryId, countries.ElementAt(rng.Next(countries.Count())).Id)
                    .With(x => x.GeneralEnquiryPhoneNumber, i.ToString().PadLeft(11, '0'))
                    .With(x => x.IconPath, "/Resources/dashboard-addExpenditures.png");
                _storage.Add(item.Create());
            }
        }

        public async Task<ApiResponse> Create(BankDetails obj)
        {
            obj.Id = _storage.Max(x => x.Id) + 1;
            _storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var item = _storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            _storage.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int id, BankDetails obj)
        {
            var existing = _storage.SingleOrDefault(x => x.Id == id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.CountryId = obj.CountryId;
            existing.GeneralEnquiryPhoneNumber = obj.GeneralEnquiryPhoneNumber;
            existing.Website = obj.Website;
            existing.IconPath = obj.IconPath;

            return await Task.FromResult(new ApiResponse(true));
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

