using System;

namespace PersonalFinanceManager.DTOs.Saving
{
    public class SavingList
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public DateTime DateSaving { get; set; }

        public decimal Amount { get; set; }

        public int TargetInternalAccountId { get; set; }

        public string TargetInternalAccountName { get; set; }
    }
}