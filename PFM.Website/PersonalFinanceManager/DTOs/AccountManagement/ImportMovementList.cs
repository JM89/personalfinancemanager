using System;
using System.Collections.Generic;

namespace PersonalFinanceManager.Api.Contracts.AccountManagement
{
    public class ImportMovementList
    {
        public int AccountId { get; set; }

        public decimal AccountCurrentBalance { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public DateTime? LastMovementRegistered { get; set; }

        public List<string> SelectedValues { get; set; }

        public List<MovementPropertyDefinition> MovementPropertyDefinitions { get; set; }
    }
}
