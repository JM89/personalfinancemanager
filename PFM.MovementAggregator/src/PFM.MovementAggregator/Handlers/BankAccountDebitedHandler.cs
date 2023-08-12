using PFM.Bank.Event.Contracts;
using PFM.MovementAggregator.Handlers.Interfaces;
using Serilog.Context;
using SerilogTimings;

namespace PFM.MovementAggregator.Handlers
{
    public class BankAccountDebitedHandler : IHandler<BankAccountDebited>
    {
        private readonly Serilog.ILogger _logger;

        public BankAccountDebitedHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> HandleEvent(BankAccountDebited evt)
        {
            using (var op = Operation.Begin("Handle {eventType} event", evt.GetType()))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                _logger.Information("Do something with debited event");

                op.Complete();
            }
            return true;
        }
    }
}
