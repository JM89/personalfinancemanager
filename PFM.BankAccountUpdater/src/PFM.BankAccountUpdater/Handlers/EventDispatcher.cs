using PFM.BankAccountUpdater.Handlers.Interfaces;
using System.Collections.ObjectModel;

namespace PFM.BankAccountUpdater.Handlers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Dictionary<Type, Collection<Delegate>> handlers;
        private readonly Serilog.ILogger _logger;

        public EventDispatcher(Serilog.ILogger logger)
        {
            handlers = new Dictionary<Type, Collection<Delegate>>();
            _logger = logger;
        }

        public void Register<TEvent>(Action<TEvent> handler)
             where TEvent : IEvent
        {
            Collection<Delegate> eventHandlers;
            if (!handlers.TryGetValue(typeof(TEvent), out eventHandlers))
            {
                eventHandlers = new Collection<Delegate>();
                handlers.Add(typeof(TEvent), eventHandlers);
            }

            eventHandlers.Add(handler);
        }

        public void Dispatch<TEvent>(TEvent e)
        {
            Collection<Delegate> eventHandlers;
            if (handlers.TryGetValue(typeof(TEvent), out eventHandlers))
            {
                foreach (var handler in eventHandlers.Cast<Action<TEvent>>())
                {
                    try
                    {
                        handler(e);
                    }
                    catch
                    {
                        _logger.Error("Unhandled exception");
                    }
                }
            }
        }
    }
}
