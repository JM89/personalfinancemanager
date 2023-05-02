namespace Services.Events.Interfaces
{
    public interface IEvent
    {
        string Id { get; }

        string StreamGroup { get; }
    }
}
