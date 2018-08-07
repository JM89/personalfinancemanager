using System;

namespace PFM.DTOs.AtmWithdraw
{
    public class AtmWithdrawDetails
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime DateExpense { get; set; }

        public string DisplayedDateExpense => this.DateExpense.ToString("dd/MM/yyyy");

        public decimal InitialAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public bool HasBeenAlreadyDebited { get; set; }
    }
}