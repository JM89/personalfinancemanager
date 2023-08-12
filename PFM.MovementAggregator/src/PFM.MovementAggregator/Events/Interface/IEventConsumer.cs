namespace PFM.MovementAggregator.Events.Interface
{
    public interface IEventConsumer
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}
