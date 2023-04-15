using System;

namespace PFM.Api.Contracts.Income
{
    public class IncomeList
    {
        public int Id { get; set; }

        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public decimal Cost { get; set; }

        public string Description { get; set; }

        public string DisplayedCost
        {
            get
            {
                return AccountCurrencySymbol + this.Cost;
            }
        }

        public DateTime DateIncome { get; set; }

        public string DisplayedDateIncome
        {
            get
            {
                return this.DateIncome.ToString("dd/MM/yyyy");
            }
        }
        
    }
}