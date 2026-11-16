using Newtonsoft.Json;
using PFM.Api.Contracts.Saving;
using PFM.Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Account;

namespace PFM.Services.ExternalServices.InMemoryStorage
{
	public class SavingInMemory : ISavingApi
	{
        private readonly IList<SavingDetails> _storage = new List<SavingDetails>();
        private readonly IBankAccountApi _accounts;
        private readonly Lazy<Task> _initialization;

        public SavingInMemory(IBankAccountApi accounts)
        {
            _accounts = accounts;
            _initialization = new Lazy<Task>(InitializeAsync);
        }

        private async Task InitializeAsync()
        {
            var savingAccounts = await GetSavingAccountsAsync();
            var rng = new Random();
            for (int i = 1; i <= 5 && savingAccounts.Count > 0; i++)
            {
                var targetSavingAccount = savingAccounts[rng.Next(savingAccounts.Count)];
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
            await _initialization.Value;
            var savingAccounts = await GetSavingAccountsAsync();
            obj.Id = _storage.Any() ? _storage.Max(x => x.Id) + 1 : 1;
            obj.TargetInternalAccountName = savingAccounts.Single(x => x.Id == obj.TargetInternalAccountId).Name;
            _storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            await _initialization.Value;
            var item = _storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            _storage.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> GetList(int accountId)
        {
            await _initialization.Value;
            var result = JsonConvert.SerializeObject(_storage.Where(x => x.AccountId == accountId).ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            await _initialization.Value;
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        private async Task<List<AccountList>> GetSavingAccountsAsync() =>
            BankAccountInMemory.ReadList<AccountList>(await _accounts.GetList("offline"), "accounts")
                .Where(account => account.IsSavingAccount).ToList();
    }
}
