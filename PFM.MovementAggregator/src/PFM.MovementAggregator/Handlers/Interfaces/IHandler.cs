using PFM.Bank.Event.Contracts.Interfaces;

namespace PFM.MovementAggregator.Handlers.Interfaces
{
    public interface IHandler<T>
        where T : IEvent
    {
        Task<bool> HandleEvent(T evt);
    }
}
