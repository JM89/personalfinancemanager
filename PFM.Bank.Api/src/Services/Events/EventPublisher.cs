﻿using EventStore.Client;
using PFM.Services.Events.Interfaces;
using System.Collections.Generic;
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
            var eventData = new EventData(
                Uuid.NewUuid(),
                typeof(T).Name,
                JsonSerializer.SerializeToUtf8Bytes(evt)
            );

            var result = await _eventStoreClient.AppendToStreamAsync(
                evt.Id,
                StreamState.Any,
                new[] { eventData },
                cancellationToken: token
            );

            return result != null;
        }
    }
}
