using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.BankAccountUpdater.ExternalServices.BankApi;
using PFM.BankAccountUpdater.Services.Interfaces;

namespace PFM.BankAccountUpdater.Services
{
    public class BankAccountService(IBankAccountApi bankAccountApi, Serilog.ILogger logger) : IBankAccountService
    {
        public async Task<bool> UpdateBalance(int bankAccountId, string userId, decimal newBalance)
        {
            AccountDetails? account = null;
            try
            {
                var response = await bankAccountApi.Get(bankAccountId);

                if (response == null || response.Data == null)
                    throw new Exception("Data is null");

                account = JsonConvert.DeserializeObject<AccountDetails>(response.Data.ToString() ?? "");

                if (account == null)
                    throw new Exception("Data is invalid");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while retrieving the bank account");
                throw;
            }

            account.CurrentBalance = newBalance;

            try
            {
                var result = await bankAccountApi.Edit(bankAccountId, userId, account);
                if (result.Data == null || (result.Data is bool && !(bool)result.Data))
                    throw new Exception("Result is invalid");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while updating the balance of the bank account");
                throw;
            }
        }
    }
}
