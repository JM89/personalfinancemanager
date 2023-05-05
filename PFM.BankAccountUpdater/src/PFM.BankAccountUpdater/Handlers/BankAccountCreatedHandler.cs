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
        private readonly IBankAccountApi _bankAccountApi;

        public BankAccountCreatedHandler(IBankAccountApi bankAccountApi, Serilog.ILogger logger)
        {
            _logger = logger;
            _bankAccountApi = bankAccountApi;
        }

        public async Task<bool> HandleEvent(BankAccountCreated evt)
        {
            using (var op = Operation.Begin("Handle BankAccountCreated event"))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                AccountDetails? account = null;
                try
                {
                    var response = await _bankAccountApi.Get(evt.Id);

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

                account.CurrentBalance = evt.CurrentBalance;

                try
                {
                    var result = await _bankAccountApi.Edit(evt.Id, evt.UserId, account);
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
