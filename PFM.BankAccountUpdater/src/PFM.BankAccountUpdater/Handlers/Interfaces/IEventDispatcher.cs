namespace PFM.BankAccountUpdater.Handlers.Interfaces
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent e);
    }
}
