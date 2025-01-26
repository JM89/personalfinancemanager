using Newtonsoft.Json;
using PFM.Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Bank;

namespace PFM.Services.ExternalServices.InMemoryStorage
{
    public class BankInMemory : IBankApi
    {
        internal readonly IList<BankDetails> Storage;
        
        public BankInMemory()
        {
            var countries = new CountryInMemory().Storage.ToList();

            var rng = new Random();
            Storage = new List<BankDetails>();
            for (int i = 0; i <= 5; i++) {
                var item = new BankDetails()
                {
                    Id = i,
                    Name = $"Bank {i}",
                    CountryId = countries.ElementAt(rng.Next(countries.Count())).Id,
                    GeneralEnquiryPhoneNumber = i.ToString().PadLeft(11, '0'),
                    IconPath = "/Resources/dashboard-addExpenditures.png"
                };
                Storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(string userId, BankDetails obj)
        {
            obj.Id = Storage.Max(x => x.Id) + 1;
            Storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var item = Storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            Storage.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int id, string userId, BankDetails obj)
        {
            var existing = Storage.SingleOrDefault(x => x.Id == id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.CountryId = obj.CountryId;
            existing.GeneralEnquiryPhoneNumber = obj.GeneralEnquiryPhoneNumber;
            existing.Website = obj.Website;
            existing.IconPath = obj.IconPath;

            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Get(string userId)
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

