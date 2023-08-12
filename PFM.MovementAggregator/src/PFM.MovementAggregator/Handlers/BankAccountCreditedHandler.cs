using PFM.Bank.Event.Contracts;
using PFM.MovementAggregator.Handlers.Constants;
using PFM.MovementAggregator.Handlers.Interfaces;
using PFM.MovementAggregator.Persistence;
using PFM.MovementAggregator.Persistence.Entities;
using Serilog.Context;
using SerilogTimings;

namespace PFM.MovementAggregator.Handlers
{
    public class BankAccountCreditedHandler : IHandler<BankAccountCredited>
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMovementAggregatorRepository _movementAggregatorRepository;

        public BankAccountCreditedHandler(Serilog.ILogger logger, IMovementAggregatorRepository movementAggregatorRepository)
        {
            _logger = logger;
            _movementAggregatorRepository = movementAggregatorRepository;
        }

        public async Task<bool> HandleEvent(BankAccountCredited evt)
        {
            using (var op = Operation.Begin("Handle {eventType} event", evt.GetType()))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                var movementAggregation = new MovementAggregation()
                {
                    BankAccountId = int.Parse(evt.StreamId.Replace("BankAccount-", "")),
                    Type = MovementTypes.Incomes,
                    Category = MovementTypes.Incomes,
                    MonthYearIdentifier = evt.OperationDate.ToString("yyyyMM"),
                    AggregatedAmount = evt.CurrentBalance - evt.PreviousBalance
                };

                var result = await _movementAggregatorRepository.Merge(movementAggregation);

                _logger.Information($"Updated {result} rows");

                op.Complete();
            }
            return true;
        }
    }
}
