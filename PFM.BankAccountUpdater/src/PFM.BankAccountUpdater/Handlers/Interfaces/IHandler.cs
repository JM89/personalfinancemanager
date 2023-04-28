namespace PFM.BankAccountUpdater.Handlers.Interfaces
{
    public interface IHandler<T>
        where T : IEvent
    {
        Task<bool> HandleEvent(T evt);
    }
}
