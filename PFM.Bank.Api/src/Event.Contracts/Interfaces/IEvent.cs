namespace PFM.Bank.Event.Contracts.Interfaces
{
    public interface IEvent
    {
        string Id { get; }

        string StreamGroup { get; }
    }
}
