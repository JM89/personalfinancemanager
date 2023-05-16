namespace PFM.CommonLibraries.Interfaces
{
    public interface IEvent
    {
        string StreamId { get; }

        string StreamGroup { get; }
    }
}
