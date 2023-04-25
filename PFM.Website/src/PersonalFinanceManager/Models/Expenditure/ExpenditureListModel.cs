using PersonalFinanceManager.Models.Resources;
using System;

namespace PersonalFinanceManager.Models.Expenditure
{
    public class ExpenditureListModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        //[DisplayName("Account")]
        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        [LocalizedDisplayName("ExpenditureDateExpenditure")]
        public DateTime DateExpenditure { get; set; }

        [LocalizedDisplayName("ExpenditureCost")]
        public decimal Cost { get; set; }

        public int TypeExpenditureId { get; set; }

        [LocalizedDisplayName("ExpenditureTypeExpenditure")]
        public string TypeExpenditureName { get; set; }

        public int PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; }

        public string PaymentMethodIconClass { get; set; }

        public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }

        [LocalizedDisplayName("ExpenditureDescription")]
        public string Description { get; set; }

        [LocalizedDisplayName("ExpenditureCost")]
        public string DisplayedCost
        {
            get
            {
                return this.AccountCurrencySymbol + this.Cost;
            }
        }

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

        [LocalizedDisplayName("ExpenditureAtmWithdraw")]
        public int? AtmWithdrawId { get; set; }

        [LocalizedDisplayName("ExpenditureTargetInternalAccount")]
        public int? TargetInternalAccountId { get; set; }
    }

}