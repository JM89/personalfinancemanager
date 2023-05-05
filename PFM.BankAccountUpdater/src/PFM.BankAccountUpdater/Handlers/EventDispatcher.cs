using PFM.Bank.Event.Contracts.Interfaces;
using PFM.BankAccountUpdater.Handlers.Interfaces;

namespace PFM.BankAccountUpdater.Handlers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Dictionary<Type, Action<IEvent>> handlers;
        private readonly IServiceCollection _services;
        private readonly Serilog.ILogger _logger;

        public EventDispatcher(Serilog.ILogger logger, IServiceCollection services)
        {
            handlers = new Dictionary<Type, Action<IEvent>>();
            _logger = logger;
            _services = services;
        }

        public EventDispatcher Register<TEvent>()
            where TEvent : IEvent
        {
            var handlerInstance = _services.BuildServiceProvider().GetRequiredService<IHandler<TEvent>>();

            handlers.Add(typeof(TEvent), e => handlerInstance.HandleEvent((TEvent)Convert.ChangeType(e, typeof(TEvent))));
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
