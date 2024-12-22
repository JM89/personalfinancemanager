using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.Income;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
	public class IncomeInMemory : IIncomeApi
	{
        private readonly IList<IncomeDetails> _storage;

        public IncomeInMemory()
        {
            _storage = new List<IncomeDetails>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new IncomeDetails()
                {
                    Id = i,
                    Description = $"Salary {i}",
                    AccountId = 1,
                    Cost = 100*i,
                    DateIncome = DateTime.UtcNow.AddDays(-i)
                };
                _storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(IncomeDetails obj)
        {
            obj.Id = _storage.Any() ? _storage.Max(x => x.Id) + 1 : 1;
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

        public async Task<ApiResponse> GetList(int accountId)
        {
            var result = JsonConvert.SerializeObject(_storage.Where(x => x.AccountId == accountId).ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }
    }
}

