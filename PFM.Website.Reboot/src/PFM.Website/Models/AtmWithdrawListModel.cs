using System;
namespace PFM.Website.Models
{
	public class AtmWithdrawListModel
	{
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public string Description => "ATM " + DisplayedDateExpense + " (Left: " + DisplayedCurrentAmount + ")";

        public DateTime DateExpense { get; set; }

        public string DisplayedDateExpense => this.DateExpense.ToString("yyyy-MM-dd");

        public decimal InitialAmount { get; set; }

        public bool IsClosed { get; set; }

        public string DisplayedInitialAmount => this.AccountCurrencySymbol + this.InitialAmount;

        public decimal CurrentAmount { get; set; }

        public string DisplayedCurrentAmount => this.AccountCurrencySymbol + this.CurrentAmount;

        public bool HasBeenAlreadyDebited { get; set; }

        public bool CanBeEdited { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}

