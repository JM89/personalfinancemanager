using PFM.Website.Utils;

namespace PFM.Website.Models
{
	public class BankAccountListModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string BankName { get; set; }

        public string BankIconPath { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal InitialBalance { get; set; }

        public string DisplayedInitialBalance
        {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(this.InitialBalance, this.CurrencySymbol);
            }
        }

        public decimal CurrentBalance { get; set; }

        public string DisplayedCurrentBalance
        {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(this.CurrentBalance, this.CurrencySymbol);
            }
        }

        public bool CanBeDeleted { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsSavingAccount { get; set; }
    }
}

