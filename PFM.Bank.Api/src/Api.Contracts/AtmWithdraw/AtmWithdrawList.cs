using PFM.Api.Contracts.Shared;
using System;

namespace PFM.Api.Contracts.AtmWithdraw
{
    public class AtmWithdrawList: ICanBeDeleted, ICanBeEdited
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public string Description => "ATM " + DisplayedDateExpense + " (Left: " + DisplayedCurrentAmount + ")";

        public DateTime DateExpense { get; set; }

        public string DisplayedDateExpense => this.DateExpense.ToString("dd/MM/yyyy");

        public decimal InitialAmount { get; set; }

        public bool IsClosed { get; set; }

        public string DisplayedInitialAmount => this.AccountCurrencySymbol + this.InitialAmount;

        public decimal CurrentAmount { get; set; }

        public string DisplayedCurrentAmount => this.AccountCurrencySymbol + this.CurrentAmount;

        public bool HasBeenAlreadyDebited { get; set; }

        public bool CanBeEdited { get; set; }

        public string CanBeEditedTooltipResourceName => "AtmWithdrawCantBeEdited";

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "AtmWithdrawCantBeDeleted";
    }
}