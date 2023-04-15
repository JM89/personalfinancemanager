using System;

namespace PersonalFinanceManager.Api.Contracts.Income
{
    public class IncomeDetails
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public decimal Cost { get; set; }

        public string Description { get; set; }
        
        public DateTime DateIncome { get; set; }

        public string DisplayedDateIncome => this.DateIncome.ToString("dd/MM/yyyy");

    }
}