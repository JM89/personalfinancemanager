using PFM.Bank.Event.Contracts.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task<bool> PublishAsync<T>(T evt, CancellationToken token) where T : IEvent;
    }
}
