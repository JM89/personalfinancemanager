using System;
using System.Collections.Generic;

namespace PFM.Api.Contracts.Salary
{
    public class SalaryDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal YearlySalary { get; set; }

        public decimal MonthlyGrossPay { get; set; }

        public decimal MonthlyNetPay { get; set; }

        public string UserId { get; set; }

        public int CurrencyId { get; set; }

        public IList<SalaryDeductionDetails> SalaryDeductions { get; set; }
        
        public int CountryId { get; set; }

        public int TaxId { get; set; }
    }
}