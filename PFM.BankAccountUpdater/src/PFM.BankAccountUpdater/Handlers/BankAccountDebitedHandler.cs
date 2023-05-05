using PFM.Bank.Event.Contracts;
using PFM.BankAccountUpdater.Handlers.Interfaces;
using PFM.BankAccountUpdater.Services.Interfaces;
using Serilog.Context;
using SerilogTimings;

namespace PFM.BankAccountUpdater.Handlers
{
    public class BankAccountDebitedHandler : IHandler<BankAccountDebited>
    {
        private readonly Serilog.ILogger _logger;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountDebitedHandler(IBankAccountService bankAccountService, Serilog.ILogger logger)
        {
            _logger = logger;
            _bankAccountService = bankAccountService;
        }

        public async Task<bool> HandleEvent(BankAccountDebited evt)
        {
            using (var op = Operation.Begin("Handle {eventType} event", evt.GetType()))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                await _bankAccountService.UpdateBalance(evt.Id, evt.UserId, evt.CurrentBalance);

                op.Complete();
            }
            return true;
        }
    }
}
