namespace PFM.Bank.Event.Contracts.Interfaces
{
    public interface IEvent
    {
        string EventId { get; }

        string StreamGroup { get; }
    }
}
