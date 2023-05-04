using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Event.Contracts;
using PFM.BankAccountUpdater.ExternalServices.BankApi;
using PFM.BankAccountUpdater.Handlers.Interfaces;
using Serilog.Context;
using SerilogTimings;

namespace PFM.BankAccountUpdater.Handlers
{
    public class BankAccountCreatedHandler : IHandler<BankAccountCreated>
    {
        private readonly Serilog.ILogger _logger;

        private static string IdProperty = "Id";
        private static string BankAccountIdProperty = "BankAccountId";
        private static string UserIdProperty = "UserId";

        private readonly IBankAccountApi _bankAccountApi;

        public BankAccountCreatedHandler(IBankAccountApi bankAccountApi, Serilog.ILogger logger)
        {
            _logger = logger;
            _bankAccountApi = bankAccountApi;
        }

        public async Task<bool> HandleEvent(BankAccountCreated evt)
        {
            var bankAccountId = 2; //evt.BankAccountId
            var userId = "1"; //evt.UserId
            var newBalance = 1000; //evt.CurrentBalance
                 
            using (var op = Operation.Begin("Handle BankAccountCreated event"))
            using (LogContext.PushProperty(IdProperty, evt.Id))
            using (LogContext.PushProperty(BankAccountIdProperty, bankAccountId))
            using (LogContext.PushProperty(UserIdProperty, userId))
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
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error while updating the balance of the bank account");
                    return false;
                }

                op.Complete();
            }
            return true;
        }
    }
}
