using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.AtmWithdraw
{
    public class AtmWithdrawListModel: ICanBeDeleted, ICanBeEdited
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountCurrencySymbol { get; set; }

        [LocalizedDisplayName("AtmWithdrawDescription")]
        public string Description
        {
            get
            {
                return "ATM " + DisplayedDateExpenditure + " (Left: " + DisplayedCurrentAmount + ")";
            }
        }

        [LocalizedDisplayName("AtmWithdrawDateExpenditure")]
        public DateTime DateExpenditure { get; set; }

        [LocalizedDisplayName("AtmWithdrawDateExpenditure")]
        public string DisplayedDateExpenditure
        {
            get
            {
                return this.DateExpenditure.ToString("dd/MM/yyyy");
            }
        }

        [LocalizedDisplayName("AtmWithdrawInitialAmount")]
        public decimal InitialAmount { get; set; }

        public bool IsClosed { get; set; }

        [LocalizedDisplayName("AtmWithdrawInitialAmount")]
        public string DisplayedInitialAmount
        {
            get
            {
                return this.AccountCurrencySymbol + this.InitialAmount;
            }
        }

        [LocalizedDisplayName("AtmWithdrawCurrentAmount")]
        public decimal CurrentAmount { get; set; }

        [LocalizedDisplayName("AtmWithdrawCurrentAmount")]
        public string DisplayedCurrentAmount
        {
            get
            {
                return this.AccountCurrencySymbol + this.CurrentAmount;
            }
        }

        [LocalizedDisplayName("AtmWithdrawHasBeenAlreadyDebited")]
        public bool HasBeenAlreadyDebited { get; set; }

        public bool CanBeEdited { get; set; }

        public string CanBeEditedTooltipResourceName
        {
            get
            {
                return "AtmWithdrawCantBeEdited";
            }
            set
            {

            }
        }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName
        {
            get
            {
                return "AtmWithdrawCantBeDeleted";
            }
            set
            {

            }
        }
    }
}