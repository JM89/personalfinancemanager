using PFM.Bank.Event.Contracts.Interfaces;
using PFM.BankAccountUpdater.Handlers.Interfaces;

namespace PFM.BankAccountUpdater.Handlers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Dictionary<Type, Action<IEvent>> handlers;
        private readonly Serilog.ILogger _logger;

        public EventDispatcher(Serilog.ILogger logger)
        {
            handlers = new Dictionary<Type, Action<IEvent>>();
            _logger = logger;
        }

        public EventDispatcher Register<TEvent>(Action<IEvent> handler)
            where TEvent : IEvent
        {
            handlers.Add(typeof(TEvent), handler);
            return this;
        }

        public void Dispatch(IEvent e)
        {
            Action<IEvent> handler;
            if (handlers.TryGetValue(e.GetType(), out handler))
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
