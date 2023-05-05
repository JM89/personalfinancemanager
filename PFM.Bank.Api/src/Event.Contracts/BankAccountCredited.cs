using PFM.Bank.Event.Contracts.Interfaces;
using System;

namespace PFM.Bank.Event.Contracts
{
    public class BankAccountCredited : IEvent
    {
        public string EventId => $"{StreamGroup}-{Id.ToString("00000000")}";

        public int Id { get; set; }

        public int CurrencyId { get; set; }

        public int BankId { get; set; }

        public decimal PreviousBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public string StreamGroup => "BankAccount";

        public string UserId { get; set; }

        public DateTime OperationDate { get; set; }

        public string OperationType { get; set; }

        public string TargetBankAccount { get; set; }

        public string MovementType { get; set; }
    }
}
