using PFM.Bank.Event.Contracts.Interfaces;

namespace PFM.MovementAggregator.Handlers.Interfaces
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent e);
    }
}
