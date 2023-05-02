using PFM.Bank.Event.Contracts.Interfaces;

namespace PFM.BankAccountUpdater.Handlers.Interfaces
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent e);
    }
}
