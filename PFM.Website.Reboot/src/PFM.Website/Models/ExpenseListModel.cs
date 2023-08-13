using System;
namespace PFM.Website.Models
{
	public class ExpenseListModel
	{
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public DateTime DateExpense { get; set; }

        public decimal Cost { get; set; }

        public int ExpenseTypeId { get; set; }

        public string ExpenseTypeName { get; set; }

        public int PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; }

        public string PaymentMethodIconClass { get; set; }

        public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }

        public string Description { get; set; }

        public string DisplayedCost
        {
            get
            {
                return this.AccountCurrencySymbol + this.Cost;
            }
        }

        public string DisplayedDateExpense
        {
            get
            {
                return this.DateExpense.ToString("yyyy-MM-dd");
            }
        }

        public bool HasBeenAlreadyDebited { get; set; }

        public int? AtmWithdrawId { get; set; }

        public int? TargetInternalAccountId { get; set; }
    }
}

