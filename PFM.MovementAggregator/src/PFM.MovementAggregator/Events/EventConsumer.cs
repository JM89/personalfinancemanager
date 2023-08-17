using EventStore.Client;
using PFM.Bank.Event.Contracts;
using PFM.Bank.Event.Contracts.Interfaces;
using PFM.MovementAggregator.Events.Interface;
using PFM.MovementAggregator.Events.Settings;
using PFM.MovementAggregator.Handlers.Interfaces;
using Polly;
using System.Text.Json;

namespace PFM.MovementAggregator.Events
{
    internal class EventConsumer: IEventConsumer
    {
        private readonly Serilog.ILogger _logger;
        private readonly EventStorePersistentSubscriptionsClient _client;
        private readonly EventStoreConsumerSettings _settings;
        private readonly AsyncPolicy _subscriptionRetryPolicy;
        private int _retryCount = 0;

        private readonly IEventDispatcher _eventDispatcher;

        private static string AssemblyName = typeof(BankAccountCreated).Assembly.GetName().Name ?? "";
        private static string EventTypeNamespace = "PFM.Bank.Event.Contracts";

        public EventConsumer(Serilog.ILogger logger, IEventDispatcher eventDispatcher, EventStorePersistentSubscriptionsClient client, EventStoreConsumerSettings settings)
        {
            _logger = logger;
            _client = client;
            _settings = settings;

            _eventDispatcher = eventDispatcher;
            _subscriptionRetryPolicy = Policy
               .Handle<Exception>()
               .WaitAndRetryAsync(_settings.MaxAttempt ?? 3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(_settings.ExponentialBackOffFactor ?? 3, retryAttempt)), (ex, nextRetry, context) =>
               {
                   _logger.Warning($"Subscription to stream failed with message {ex.Message}, retrying in {nextRetry.TotalSeconds} seconds... ({++_retryCount}/{_settings.MaxAttempt})");
               });
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            try
            {
                var subscription = await _subscriptionRetryPolicy.ExecuteAsync((stoppingToken) =>
                    _client.SubscribeToStreamAsync(
                        _settings.StreamName ?? "UnknownStreamName",
                        _settings.GroupName ?? "UnknownGroupName",
                        ResolveAsync, 
                        DoWhenConnectionDropped, 
                        cancellationToken: stoppingToken), 
                   stoppingToken
                );

                _logger.Information($"Subscription to stream {_settings.StreamName} and group {_settings.GroupName} was successful", subscription.SubscriptionId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unhandled error while subscribing to stream {_settings.StreamName} and group {_settings.GroupName}");
                throw;
            }
        }

        private async Task ResolveAsync(PersistentSubscription subscription, ResolvedEvent evt, int? retryCount, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
                return;

            var type = Type.GetType($"{EventTypeNamespace}.{evt.Event.EventType}, {AssemblyName}");

            if (type == null)
            {
                _logger.Debug($"Type '{evt.Event.EventType}' not handled, ignoring the event.");
                await subscription.Ack(evt);
                return;
            }

            var receivedEvent = JsonSerializer.Deserialize(System.Text.Encoding.UTF8.GetString(evt.Event.Data.Span), type) as IEvent;

            if (receivedEvent == null)
            {
                _logger.Error("Received event is null");
                throw new ArgumentNullException("Received event is null");
            }

            _eventDispatcher.Dispatch(receivedEvent);

            await subscription.Ack(evt);
        }
         
        private void DoWhenConnectionDropped(PersistentSubscription subscription, SubscriptionDroppedReason dropReason, Exception? exception)
        {
            _logger.Error(exception, $"Subscription for stream {_settings.StreamName} and group {_settings.GroupName} dropped due to {dropReason}.");
        }
    }
}
