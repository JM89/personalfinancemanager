using PFM.Bank.Event.Contracts;
using PFM.MovementAggregator.Handlers.Interfaces;
using Serilog.Context;
using SerilogTimings;

namespace PFM.MovementAggregator.Handlers
{
    public class BankAccountCreditedHandler : IHandler<BankAccountCredited>
    {
        private readonly Serilog.ILogger _logger;

        public BankAccountCreditedHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> HandleEvent(BankAccountCredited evt)
        {
            using (var op = Operation.Begin("Handle {eventType} event", evt.GetType()))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                _logger.Information("Do something with credited event");

                op.Complete();
            }
            return true;
        }
    }
}
