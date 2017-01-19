using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.Saving
{
    public class SavingListModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        [LocalizedDisplayName("SavingDateSaving")]
        public DateTime DateSaving { get; set; }

        [LocalizedDisplayName("SavingAmount")]
        public decimal Amount { get; set; }

        [LocalizedDisplayName("SavingAmount")]
        public string DisplayedAmount => DecimalFormatHelper.GetDisplayDecimalValue(Amount, AccountCurrencySymbol);

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

        public int TargetInternalAccountId { get; set; }

        [LocalizedDisplayName("SavingTargetInternalAccount")]
        public string TargetInternalAccountName { get; set; }
    }
}