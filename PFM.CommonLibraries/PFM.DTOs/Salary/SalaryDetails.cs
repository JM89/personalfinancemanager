using System;
using System.Collections.Generic;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Salary
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

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public IList<SalaryDeductionDetails> SalaryDeductions { get; set; }
        
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public int TaxId { get; set; }

        public IList<SelectListItem> AvailableTaxes { get; set; }
    }
}