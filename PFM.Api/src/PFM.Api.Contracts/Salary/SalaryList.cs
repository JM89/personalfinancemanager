using System;

namespace PFM.Api.Contracts.Salary
{
    public class SalaryList
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal YearlySalary { get; set; }

        public decimal MonthlyGrossPay { get; set; }

        public decimal MonthlyNetPay { get; set; }

        public string CurrencySymbol { get; set; }

        public string CountryName { get; set; }
    }
}