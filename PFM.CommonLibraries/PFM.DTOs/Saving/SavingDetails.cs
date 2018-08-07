using System;
using System.Collections.Generic;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Saving
{
    public class SavingDetails
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime DateSaving { get; set; }

        public decimal Amount { get; set; }

        public int? TargetInternalAccountId { get; set; }
        
        public string TargetInternalAccountName { get; set; }

        public string DisplayedDateSaving => DateTimeFormatHelper.GetDisplayDateValue(this.DateSaving);

        public string Description
        {
            get
            {
                return "Saving " + DisplayedDateSaving;
            }
        }

        public IList<SelectListItem> AvailableInternalAccounts { get; set; }

        public int? GeneratedIncomeId { get; set; }
    }
}