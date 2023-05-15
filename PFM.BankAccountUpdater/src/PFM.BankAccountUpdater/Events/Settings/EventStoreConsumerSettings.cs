namespace PFM.BankAccountUpdater.Events.Settings
{
    public class EventStoreConsumerSettings
    {
        public string? GroupName { get; set; }
        public string? StreamName { get; set; }
        public int? MaxAttempt { get; set; }
        public int? ExponentialBackOffFactor { get; set; }
    }
}
