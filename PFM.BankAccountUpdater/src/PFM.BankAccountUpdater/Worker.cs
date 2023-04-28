using PFM.BankAccountUpdater.Events.Interface;

namespace PFM.BankAccountUpdater
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