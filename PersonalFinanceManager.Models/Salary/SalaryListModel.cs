using System;
using System.ComponentModel.DataAnnotations;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;

namespace PersonalFinanceManager.Models.Salary
{
    public class SalaryListModel
    {
        [LocalizedDisplayName("SalaryId")]
        public int Id { get; set; }

        [LocalizedDisplayName("SalaryDescription")]
        public string Description { get; set; }

        [LocalizedDisplayName("SalaryStartDate")]
        public DateTime StartDate { get; set; }

        [LocalizedDisplayName("SalaryEndDate")]
        public DateTime? EndDate { get; set; }

        [LocalizedDisplayName("SalaryYearlySalary")]
        public decimal YearlySalary { get; set; }

        [LocalizedDisplayName("SalaryMonthlyGrossPay")]
        public decimal MonthlyGrossPay { get; set; }

        [LocalizedDisplayName("SalaryMonthlyNetPay")]
        public decimal MonthlyNetPay { get; set; }

        [LocalizedDisplayName("SalaryCurrency")]
        public string CurrencySymbol { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public string DisplayedYearlySalary => DecimalFormatHelper.GetDisplayDecimalValue(YearlySalary, CurrencySymbol);

        public string DisplayedMonthlyGrossPay => DecimalFormatHelper.GetDisplayDecimalValue(MonthlyGrossPay, CurrencySymbol);

        public string DisplayedMonthlyNetPay => DecimalFormatHelper.GetDisplayDecimalValue(MonthlyNetPay, CurrencySymbol);

    }
}