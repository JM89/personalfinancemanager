﻿using PFM.Bank.Event.Contracts.Interfaces;
using System;

namespace PFM.Bank.Event.Contracts
{
    public class BankAccountDebited : IEvent
    {
        public string Id => $"{StreamGroup}-{UserId}-{BankCode}-{CurrencyCode}";

        public string CurrencyCode { get; set; }

        public string BankCode { get; set; }

        public decimal PreviousBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public string StreamGroup => "BankAccount";

        public string UserId { get; set; }

        public DateTime OperationDate { get; set; }

        public string OperationType { get; set; }

        public string TargetBankAccount { get; set; }
    }
}