using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Bank.Api.Contracts.Currency;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class BankAccountInMemory : IBankAccountApi
    {
        private IList<AccountDetails> _storage;
        private IList<CurrencyDetails> _currencies = new CurrencyInMemory()._storage.ToList();
        private IList<BankDetails> _banks = new BankInMemory()._storage.ToList();

        public BankAccountInMemory()
        {
            var rng = new Random();
            _storage = new List<AccountDetails>();
            for (int i = 0; i <= 5; i++) {
                var item = new AccountDetails() {
                    Id = i,
                    Name = $"Current account {i}",
                    BankId = _banks.ElementAt(rng.Next(_banks.Count())).Id,
                    CurrencyId = _currencies.ElementAt(rng.Next(_currencies.Count())).Id,
                    IsFavorite = i == 0,
                    IsSavingAccount = false
                };
                _storage.Add(item);
            }
            for (int i = 5; i <= 10; i++)
            {
                var item = new AccountDetails()
                {
                    Id = i,
                    Name = $"Saving account {i}",
                    BankId = _banks.ElementAt(rng.Next(_banks.Count())).Id,
                    CurrencyId = _currencies.ElementAt(rng.Next(_currencies.Count())).Id,
                    IsFavorite = false,
                    IsSavingAccount = true
                };
                _storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(string userId, AccountDetails obj)
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

        public Task<ApiResponse> Edit(int id, string userId, AccountDetails obj)
        {
            throw new NotImplementedException("TODO: for the time being, we will delete and create an account instead of editing.");
        }

        public async Task<ApiResponse> GetList(string userId)
        {
            var extendedList = _storage.Select(x => new AccountList() {
                Id = x.Id,
                Name = x.Name,
                BankName = _banks.Single(b => b.Id == x.BankId).Name,
                CurrencyName = _currencies.Single(c => c.Id == x.CurrencyId).Name,
                InitialBalance = 1000,
                CurrentBalance = 1200,
                IsSavingAccount = x.IsSavingAccount,
                IsFavorite = x.IsFavorite,
                CanBeDeleted = true
            });
            var result = JsonConvert.SerializeObject(extendedList);
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> SetAsFavorite(int id)
        {
            foreach (var existingItem in _storage)
            {
                existingItem.IsFavorite = false;
            }

            var item = _storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            item.IsFavorite = true;

            return await Task.FromResult(new ApiResponse(true));
        }
    }
}

