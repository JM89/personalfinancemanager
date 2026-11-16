using Newtonsoft.Json;
using PFM.Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Bank.Api.Contracts.Currency;

namespace PFM.Services.ExternalServices.InMemoryStorage
{
    public class BankAccountInMemory : IBankAccountApi
    {
        internal readonly IList<AccountDetails> Storage;
        private readonly IBankApi _banks;
        private readonly ICurrencyApi _currencies;
        private readonly Lazy<Task> _initialization;

        public BankAccountInMemory(IBankApi banks, ICurrencyApi currencies)
        {
            _banks = banks;
            _currencies = currencies;
            Storage = new List<AccountDetails>();
            _initialization = new Lazy<Task>(InitializeAsync);
        }

        private async Task InitializeAsync()
        {
            var banks = await GetBanksAsync("offline");
            var currencies = await GetCurrenciesAsync();
            if (banks.Count == 0 || currencies.Count == 0)
                throw new InvalidOperationException("Cannot initialize demo accounts without banks and currencies.");
            var rng = new Random();
            for (int i = 0; i <= 4; i++) {
                var currency = currencies[rng.Next(currencies.Count)];
                var item = new AccountDetails() {
                    Id = i,
                    Name = $"Current account {i}",
                    BankId = banks[rng.Next(banks.Count)].Id,
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
                    BankId = banks[rng.Next(banks.Count)].Id,
                    CurrencyId = currencies[rng.Next(currencies.Count)].Id,
                    IsFavorite = false,
                    IsSavingAccount = true
                };
                Storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(string userId, AccountDetails obj)
        {
            await _initialization.Value;
            obj.Id = Storage.Max(x => x.Id) + 1;
            Storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            await _initialization.Value;
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
            await _initialization.Value;
            var banks = await GetBanksAsync(userId);
            var currencies = await GetCurrenciesAsync();
            var extendedList = Storage.Select(x => new AccountList() {
                Id = x.Id,
                Name = x.Name,
                BankName = banks.Single(b => b.Id == x.BankId).Name,
                CurrencyName = currencies.Single(c => c.Id == x.CurrencyId).Name,
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
            await _initialization.Value;
            var item = JsonConvert.SerializeObject(Storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> SetAsFavorite(int id)
        {
            await _initialization.Value;
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

        private async Task<List<BankDetails>> GetBanksAsync(string userId) =>
            ReadList<BankDetails>(await _banks.Get(userId), "banks");

        private async Task<List<CurrencyDetails>> GetCurrenciesAsync() =>
            ReadList<CurrencyDetails>(await _currencies.GetList(), "currencies");

        internal static List<T> ReadList<T>(ApiResponse response, string name)
        {
            if (response.Data is null)
                throw new InvalidOperationException($"Could not load {name}: {string.Join("; ", response.Errors?.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}") ?? Array.Empty<string>())}");
            return JsonConvert.DeserializeObject<List<T>>(response.Data.ToString()!)
                ?? throw new InvalidOperationException($"The {name} response was empty.");
        }
    }
}
