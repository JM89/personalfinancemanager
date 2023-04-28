using System.Threading.Tasks;
using System.Threading;

namespace PFM.Services.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task<bool> PublishAsync<T>(T evt, CancellationToken token) where T : IEvent;
    }
}
