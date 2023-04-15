using System;
using System.Collections.Generic;

namespace PFM.Services.DTOs.Saving
{
    public class SavingDetails
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime DateSaving { get; set; }

        public decimal Amount { get; set; }

        public int? TargetInternalAccountId { get; set; }
        
        public string TargetInternalAccountName { get; set; }

        public int? GeneratedIncomeId { get; set; }

        public string Description { get; set; }
    }
}