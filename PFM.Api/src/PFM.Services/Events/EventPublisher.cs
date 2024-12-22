using System;
using System.Collections.Generic;
using System.Diagnostics;
using EventStore.Client;
using PFM.Bank.Event.Contracts.Interfaces;
using PFM.Services.Events.Interfaces;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PFM.Services.Events
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
            var metadata = new Dictionary<string, object>()
            {
                { "TraceId", Activity.Current?.Id ?? string.Empty },
            };
            
            var eventData = new EventData(
                Uuid.NewUuid(),
                typeof(T).Name,
                JsonSerializer.SerializeToUtf8Bytes(evt),
                metadata: JsonSerializer.SerializeToUtf8Bytes(metadata)
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
