using PFM.Bank.Event.Contracts;
using PFM.MovementAggregator.Handlers.Constants;
using PFM.MovementAggregator.Handlers.Interfaces;
using PFM.MovementAggregator.Persistence;
using PFM.MovementAggregator.Persistence.Entities;
using Serilog.Context;
using SerilogTimings;

namespace PFM.MovementAggregator.Handlers
{
    public class BankAccountDebitedHandler : IHandler<BankAccountDebited>
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMovementAggregatorRepository _movementAggregatorRepository;

        public BankAccountDebitedHandler(Serilog.ILogger logger, IMovementAggregatorRepository movementAggregatorRepository)
        {
            _logger = logger;
            _movementAggregatorRepository = movementAggregatorRepository;
        }

        public async Task<bool> HandleEvent(BankAccountDebited evt)
        {
            using (var op = Operation.Begin("Handle {eventType} event", evt.GetType()))
            using (LogContext.PushProperty(nameof(evt.StreamId), evt.StreamId))
            using (LogContext.PushProperty(nameof(evt.Id), evt.Id))
            using (LogContext.PushProperty(nameof(evt.UserId), evt.UserId))
            {
                var movementAggregation = new MovementAggregation()
                {
                    BankAccountId = evt.Id,
                    MonthYearIdentifier = evt.OperationDate.ToString("yyyyMM"),
                    AggregatedAmount = evt.PreviousBalance - evt.CurrentBalance
                };

                if (evt.TargetBankAccount != null)
                {
                    movementAggregation.Type = MovementTypes.Savings;
                    movementAggregation.Category = MovementTypes.Savings;
                }
                else
                {
                    movementAggregation.Type = MovementTypes.Expenses;
                    movementAggregation.Category = evt.MovementType ?? "Unknown";
                }

                var result = await _movementAggregatorRepository.Merge(movementAggregation);

                _logger.Information($"Updated {result} rows");

                op.Complete();
            }
            return true;
        }
    }
}
