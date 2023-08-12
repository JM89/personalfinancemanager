using PFM.MovementAggregator.Events.Interface;

namespace PFM.MovementAggregator
{
    public class Worker : BackgroundService
    {
        private readonly IEventConsumer _eventConsumer;

        public Worker(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventConsumer.StartAsync(stoppingToken);
        }
    }
}