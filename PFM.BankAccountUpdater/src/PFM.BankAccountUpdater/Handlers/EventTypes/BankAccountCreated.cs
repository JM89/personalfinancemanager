using PFM.BankAccountUpdater.Handlers.Interfaces;

namespace PFM.BankAccountUpdater.Handlers.EventTypes
{
    public class BankAccountCreated : IEvent
    {
        public string Id { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}
