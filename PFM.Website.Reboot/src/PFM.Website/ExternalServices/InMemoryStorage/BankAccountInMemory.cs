using PFM.Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Bank.Api.Contracts.Currency;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class BankAccountInMemory : IBankAccountApi
    {
        internal readonly IList<AccountDetails> Storage;
        private readonly IList<CurrencyDetails> _currencies = new CurrencyInMemory().Storage.ToList();
        private readonly IList<BankDetails> _banks = new BankInMemory().Storage.ToList();

        public BankAccountInMemory()
        {
            var rng = new Random();
            Storage = new List<AccountDetails>();
            for (int i = 0; i <= 4; i++) {
                var currency = _currencies.ElementAt(rng.Next(_currencies.Count()));
                var item = new AccountDetails() {
                    Id = i,
                    Name = $"Current account {i}",
                    BankId = _banks.ElementAt(rng.Next(_banks.Count())).Id,
                    CurrencyId = currency.Id,
                    CurrencyName = currency.Name,
                    CurrencySymbol = currency.Symbol,
                    IsFavorite = i == 1,
                    IsSavingAccount = false
                };
                Storage.Add(item);
            }
            for (int i = 5; i <= 9; i++)
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
                Storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(string userId, AccountDetails obj)
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

        public Task<ApiResponse> Edit(int id, string userId, AccountDetails obj)
        {
            throw new NotImplementedException("TODO: for the time being, we will delete and create an account instead of editing.");
        }

        public async Task<ApiResponse> GetList(string userId)
        {
            var extendedList = Storage.Select(x => new AccountList() {
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
            var item = JsonConvert.SerializeObject(Storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> SetAsFavorite(int id)
        {
            foreach (var existingItem in Storage)
            {
                existingItem.IsFavorite = false;
            }

            var item = Storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            item.IsFavorite = true;

            return await Task.FromResult(new ApiResponse(true));
        }
    }
}

