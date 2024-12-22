using PFM.Bank.Event.Contracts;
using PFM.BankAccountUpdater.Handlers.Interfaces;
using PFM.BankAccountUpdater.Services.Interfaces;
using Serilog.Context;
using SerilogTimings;

namespace PFM.BankAccountUpdater.Handlers
{
    public class BankAccountCreditedHandler(IBankAccountService bankAccountService, Serilog.ILogger logger)
        : IHandler<BankAccountCredited>
    {
        private readonly Serilog.ILogger _logger = logger;

        public async Task<bool> HandleEvent(BankAccountCredited evt)
        {
            using (var op = Operation.Begin("Handle {eventType} event", evt.GetType()))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                await bankAccountService.UpdateBalance(evt.Id, evt.UserId, evt.CurrentBalance);

                op.Complete();
            }
            return true;
        }
    }
}
