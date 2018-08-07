using PFM.DTOs.PaymentMethod;
using System;
using System.Collections.Generic;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Expense
{
    public class ExpenseDetails
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime DateExpense { get; set; }

        public decimal Cost { get; set; }

        public int TypeExpenseId { get; set; }

        public int PaymentMethodId { get; set; }

        public string Description { get; set; }

        public int? AtmWithdrawId { get; set; }

        public int? TargetInternalAccountId { get; set; }

        public string DisplayedDateExpense => DateTimeFormatHelper.GetDisplayDateValue(this.DateExpense);

        public bool HasBeenAlreadyDebited { get; set; }

        public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }
       
        public IList<SelectListItem> AvailableExpenseTypes { get; set; }
        
        public IList<PaymentMethodList> AvailablePaymentMethods { get; set; }
        
        public IList<SelectListItem> AvailableAtmWithdraws { get; set; }

        public IList<SelectListItem> AvailableInternalAccounts { get; set; }

        public int? GeneratedIncomeId { get; set; }
    }

}