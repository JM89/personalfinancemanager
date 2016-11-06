using PersonalFinanceManager.Models.Resources;
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
    public class AtmWithdrawEditModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
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

        [Required]
        [LocalizedDisplayName("AtmWithdrawInitialAmount")]
        public decimal InitialAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        [LocalizedDisplayName("AtmWithdrawHasBeenAlreadyDebited")]
        public bool HasBeenAlreadyDebited { get; set; }
    }
}