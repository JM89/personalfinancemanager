namespace PFM.BankAccountUpdater.Events.Interface
{
    public interface IEventConsumer
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}
