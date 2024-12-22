using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.AtmWithdraw;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
	public class AtmWithdrawInMemory : IAtmWithdrawApi
	{
        internal readonly IList<AtmWithdrawDetails> Storage;

        public AtmWithdrawInMemory()
        {
            var rng = new Random();
            Storage = new List<AtmWithdrawDetails>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new AtmWithdrawDetails()
                {
                    Id = i,
                    AccountId = 1,
                    InitialAmount = 100 * i,
                    CurrentAmount = 100 * i,
                    DateExpense = DateTime.UtcNow.AddDays(-i),
                    HasBeenAlreadyDebited = true
                };
                Storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(AtmWithdrawDetails obj)
        {
            obj.Id = Storage.Any() ? Storage.Max(x => x.Id) + 1 : 1;
            obj.CurrentAmount = obj.InitialAmount;
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

        public async Task<ApiResponse> GetList(int accountId)
        {
            
            var result = JsonConvert.SerializeObject(Storage.Where(x => x.AccountId == accountId).ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(Storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> CloseAtmWithdraw(int id)
        {
            // Do nothing - property not available in details object
            return await Task.FromResult(new ApiResponse(false));
        }

        public async Task<ApiResponse> ChangeDebitStatus(int id, bool debitStatus)
        {
            var item = Storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            item.HasBeenAlreadyDebited = !debitStatus;

            return await Task.FromResult(new ApiResponse(true));
        }
    }
}

