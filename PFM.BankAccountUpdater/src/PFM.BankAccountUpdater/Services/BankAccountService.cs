using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.BankAccountUpdater.ExternalServices.BankApi;
using PFM.BankAccountUpdater.Services.Interfaces;

namespace PFM.BankAccountUpdater.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IBankAccountApi _bankAccountApi;

        public BankAccountService(IBankAccountApi bankAccountApi, Serilog.ILogger logger)
        {
            _logger = logger;
            _bankAccountApi = bankAccountApi;
        }

        public async Task<bool> UpdateBalance(int bankAccountId, string userId, decimal newBalance)
        {
            AccountDetails? account = null;
            try
            {
                var response = await _bankAccountApi.Get(bankAccountId);

                if (response == null || response.Data == null)
                    throw new Exception("Data is null");

                account = JsonConvert.DeserializeObject<AccountDetails>(response.Data.ToString());

                if (account == null)
                    throw new Exception("Data is invalid");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while retrieving the bank account");
                return false;
            }

            account.CurrentBalance = newBalance;

            try
            {
                var result = await _bankAccountApi.Edit(bankAccountId, userId, account);
                if (result.Data == null || (result.Data is bool && !(bool)result.Data))
                    throw new Exception("Result is invalid");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating the balance of the bank account");
                return false;
            }
        }
    }
}
