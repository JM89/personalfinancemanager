using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Api.Contracts.Bank;
using System.Net.Http.Json;

namespace PFM.IntegrationTests.TestSetup
{
    public static class DataInitializer
    {
        public static async Task<BankDetails> CreateNewBank(this HttpClient httpClient)
        {
            var bank = new BankDetails()
            {
                Name = "Bank Account",
                CountryId = 1,
                GeneralEnquiryPhoneNumber = "00000000000",
                DisplayIconFlags = DisplayIcon.None,
                IconPath = "icon/path.png",
                Website = "http://website.com"
            };

            var response = await httpClient.PostAsJsonAsync("/api/Bank", bank);
            var responseApiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            var data = JsonConvert.DeserializeObject<BankDetails>(responseApiResponse?.Data.ToString() ?? "");

            ArgumentNullException.ThrowIfNull(data);

            return data;
        }

        public static async Task<AccountDetails> CreateNewBankAccount(this HttpClient httpClient, bool isSavingAccount = false, int? optionalBankId = null)
        {
            if (!optionalBankId.HasValue)
            {
                optionalBankId = (await httpClient.CreateNewBank()).Id;
            }

            var bank = new AccountDetails()
            {
                Name = "Account",
                BankId = optionalBankId.Value,
                CurrencyId = 1,
                IsFavorite = false,
                IsSavingAccount = isSavingAccount,
                InitialBalance = 0
            };

            var response = await httpClient.PostAsJsonAsync("/api/BankAccount", bank);
            var responseApiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            var data = JsonConvert.DeserializeObject<AccountDetails>(responseApiResponse?.Data.ToString() ?? "");

            ArgumentNullException.ThrowIfNull(data);

            return data;
        }
    }
}
