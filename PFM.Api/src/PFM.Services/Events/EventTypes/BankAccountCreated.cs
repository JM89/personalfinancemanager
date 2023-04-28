using PFM.Services.Events.Interfaces;

namespace PFM.Services.Events.EventTypes
{
    internal class BankAccountCreated: IEvent
    {
        public string Id => $"{StreamGroup}-{BankCode}-{CurrencyCode}";

        public string CurrencyCode { get; set; }

        public string BankCode { get; set; }

        public decimal CurrentBalance { get; set; }

        public string StreamGroup => "BankAccount";
    }
}
