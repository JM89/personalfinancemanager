using PFM.CommonLibraries.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using PFM.CommonLibraries.EventPublisher.Interfaces;
using EventStore.Client;
using System.Text.Json;

namespace PFM.CommonLibraries.EventPublisher.Implementations
{
    public class EventPublisher : IEventPublisher
    {
        private readonly EventStoreClient _eventStoreClient;

        public EventPublisher(EventStoreClient eventStorePublisher)
        {
            _eventStoreClient = eventStorePublisher;
        }

        public async Task<bool> PublishAsync<T>(T evt, CancellationToken token)
            where T : IEvent
        {
            var eventData = new EventData(
                Uuid.NewUuid(),
                typeof(T).Name,
                JsonSerializer.SerializeToUtf8Bytes(evt)
            );

            var result = await _eventStoreClient.AppendToStreamAsync(
                evt.StreamId,
                StreamState.Any,
                new[] { eventData },
                cancellationToken: token
            );

            return result != null;
        }
    }
}
