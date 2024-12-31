namespace PFM.Bank.Event.Contracts.Interfaces
{
    public interface IEvent
    {
        string StreamId { get; }

        string StreamGroup { get; }
    }
}
