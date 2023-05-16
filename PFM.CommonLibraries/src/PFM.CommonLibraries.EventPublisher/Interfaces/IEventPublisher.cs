using PFM.CommonLibraries.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PFM.CommonLibraries.EventPublisher.Interfaces
{
    public interface IEventPublisher
    {
        Task<bool> PublishAsync<T>(T evt, CancellationToken token) where T : IEvent;
    }
}
