using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.Saving
{
    public class SavingEditModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [LocalizedDisplayName("SavingDateSaving")]
        [Required]
        public DateTime DateSaving { get; set; }

        [LocalizedDisplayName("SavingAmount")]
        [Required]
        public decimal Amount { get; set; }

        [LocalizedDisplayName("SavingTargetInternalAccount")]
        public int? TargetInternalAccountId { get; set; }

        [LocalizedDisplayName("SavingDateSaving")]
        public string DisplayedDateSaving => DateTimeFormatHelper.GetDisplayDateValue(this.DateSaving);

        [LocalizedDisplayName("SavingDescription")]
        public string Description
        {
            get
            {
                return "Saving " + DisplayedDateSaving;
            }
        }

        public IList<SelectListItem> AvailableInternalAccounts { get; set; }
    }

}