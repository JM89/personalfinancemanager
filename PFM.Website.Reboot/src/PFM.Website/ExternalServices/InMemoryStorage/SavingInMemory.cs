using PFM.Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.Saving;
using PFM.Bank.Api.Contracts.Account;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
	public class SavingInMemory : ISavingApi
	{
        private readonly IList<SavingDetails> _storage;
        private readonly IList<AccountDetails> _savingBankAccounts = new BankAccountInMemory().Storage.Where(x => x.IsSavingAccount).ToList();

        public SavingInMemory()
        {
            var rng = new Random();
            _storage = new List<SavingDetails>();
            for (int i = 1; i <= 5; i++)
            {
                var targetSavingAccount = _savingBankAccounts.ElementAt(rng.Next(_savingBankAccounts.Count()));
                var item = new SavingDetails()
                {
                    Id = i,
                    Description = $"Saving {i}",
                    AccountId = 1,
                    Amount = 100 * i,
                    DateSaving = DateTime.UtcNow.AddDays(-i),
                    TargetInternalAccountId = targetSavingAccount.Id,
                    TargetInternalAccountName = targetSavingAccount.Name
                };
                _storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(SavingDetails obj)
        {
            obj.Id = _storage.Any() ? _storage.Max(x => x.Id) + 1 : 1;
            obj.TargetInternalAccountName = _savingBankAccounts.Single(x => x.Id == obj.TargetInternalAccountId).Name;
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

