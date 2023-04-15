using PFM.Services.DTOs.PaymentMethod;
using System;
using System.Collections.Generic;

namespace PersonalFinanceManager.DTOs.Expense
{
    public class ExpenseDetails
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime DateExpense { get; set; }

        public decimal Cost { get; set; }

        public int ExpenseTypeId { get; set; }

        public int PaymentMethodId { get; set; }

        public string Description { get; set; }

        public int? AtmWithdrawId { get; set; }

        public int? TargetInternalAccountId { get; set; }

        public bool HasBeenAlreadyDebited { get; set; }

        public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }
              
        public IList<PaymentMethodList> AvailablePaymentMethods { get; set; }
        
        public int? GeneratedIncomeId { get; set; }
    }

}