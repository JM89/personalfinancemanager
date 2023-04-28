using PFM.BankAccountUpdater.Handlers.EventTypes;
using PFM.BankAccountUpdater.Handlers.Interfaces;
using Serilog.Context;
using SerilogTimings;

namespace PFM.BankAccountUpdater.Handlers
{
    public class BankAccountCreatedHandler : IHandler<BankAccountCreated>
    {
        private readonly Serilog.ILogger _logger;

        private static string IdProperty = "Id";

        public BankAccountCreatedHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public Task<bool> HandleEvent(BankAccountCreated evt)
        {
            using (var op = Operation.Begin("Handle BankAccountCreated event"))
            using (LogContext.PushProperty(IdProperty, evt.Id))
            {
                _logger.Information("Done!");
                op.Complete();
            }
            return Task.FromResult(true);
        }
    }
}
