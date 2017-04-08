using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Utils.Utils;

namespace PersonalFinanceManager.Models.Salary
{
    public class SalaryEditModel
    {
        public int Id { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryDescription")]
        public string Description { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryStartDate")]
        public DateTime StartDate { get; set; }

        [LocalizedDisplayName("SalaryEndDate")]
        public DateTime? EndDate { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryYearlySalary")]
        public decimal YearlySalary { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryMonthlyGrossPay")]
        public decimal MonthlyGrossPay { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryMonthlyNetPay")]
        public decimal MonthlyNetPay { get; set; }

        public string UserId { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryCurrency")]
        public int CurrencyId { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        [LocalizedDisplayName("SalaryDeductions")]
        public IList<SalaryDeductionEditModel> SalaryDeductions { get; set; }
        
        [Required]
        [LocalizedDisplayName("SalaryCountry")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        [Required]
        [LocalizedDisplayName("SalaryTax")]
        public int TaxId { get; set; }

        public IList<SelectListItem> AvailableTaxes { get; set; }
    }
}