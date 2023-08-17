using PFM.Website.Utils;

namespace PFM.Website.Models
{
	public class SavingListModel
	{
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public DateTime DateSaving { get; set; }

        public decimal Amount { get; set; }

        public string DisplayedAmount => DecimalFormatHelper.GetDisplayDecimalValue(Amount, AccountCurrencySymbol);

        public string DisplayedDateSaving => DateTimeFormatHelper.GetDisplayDateValue(this.DateSaving);

        public string Description
        {
            get
            {
                return "Saving " + DisplayedDateSaving;
            }
        }

        public int TargetInternalAccountId { get; set; }

        public string TargetInternalAccountName { get; set; }
    }
}

