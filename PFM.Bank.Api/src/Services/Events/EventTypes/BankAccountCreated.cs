using Services.Events.Interfaces;

namespace Services.Events.EventTypes
{
    internal class BankAccountCreated: IEvent
    {
        public string Id => $"{StreamGroup}-{UserId}-{BankCode}-{CurrencyCode}";

        public string CurrencyCode { get; set; }

        public string BankCode { get; set; }

        public decimal CurrentBalance { get; set; }

        public string StreamGroup => "BankAccount";

        public string UserId { get; set; }
    }
}
