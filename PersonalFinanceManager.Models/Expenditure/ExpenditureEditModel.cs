using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.Expenditure
{
    public class ExpenditureEditModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [LocalizedDisplayName("ExpenditureDateExpenditure")]
        [Required]
        public DateTime DateExpenditure { get; set; }

        [LocalizedDisplayName("ExpenditureCost")]
        [Required]
        public decimal Cost { get; set; }

        [LocalizedDisplayName("ExpenditureTypeExpenditure")]
        [Required]
        public int TypeExpenditureId { get; set; }

        [LocalizedDisplayName("ExpenditurePaymentMethod")]
        [Required]
        public int PaymentMethodId { get; set; }

        [LocalizedDisplayName("ExpenditureDescription")]
        [Required]
        public string Description { get; set; }

        [LocalizedDisplayName("ExpenditureAtmWithdraw")]
        public int? AtmWithdrawId { get; set; }

        [LocalizedDisplayName("ExpenditureTargetInternalAccount")]
        public int? TargetInternalAccountId { get; set; }

        [LocalizedDisplayName("ExpenditureDateExpenditure")]
        public string DisplayedDateExpenditure
        {
            get
            {
                return this.DateExpenditure.ToString("dd/MM/yyyy");
            }
        }

        [LocalizedDisplayName("ExpenditureHasBeenAlreadyDebited")]
        public bool HasBeenAlreadyDebited { get; set; }

        public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }

        //public IList<SelectListItem> AvailableAccounts { get; set; }
        
        public IList<SelectListItem> AvailableExpenditureTypes { get; set; }
        
        public IList<PaymentMethodListModel> AvailablePaymentMethods { get; set; }
        
        public IList<SelectListItem> AvailableAtmWithdraws { get; set; }

        public IList<SelectListItem> AvailableInternalAccounts { get; set; }
    }

}