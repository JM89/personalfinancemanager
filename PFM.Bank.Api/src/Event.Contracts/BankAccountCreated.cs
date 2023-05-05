using PFM.Bank.Event.Contracts.Interfaces;

namespace PFM.Bank.Event.Contracts
{
    public class BankAccountCreated: IEvent
    {
        public string EventId => $"{StreamGroup}-{Id.ToString("00000000")}";

        public int Id { get; set; }

        public int CurrencyId { get; set; }

        public int BankId { get; set; }

        public decimal CurrentBalance { get; set; }

        public string StreamGroup => "BankAccount";

        public string UserId { get; set; }
    }
}
