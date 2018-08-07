using System;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Salary
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

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public string DisplayedYearlySalary => DecimalFormatHelper.GetDisplayDecimalValue(YearlySalary, CurrencySymbol);

        public string DisplayedMonthlyGrossPay => DecimalFormatHelper.GetDisplayDecimalValue(MonthlyGrossPay, CurrencySymbol);

        public string DisplayedMonthlyNetPay => DecimalFormatHelper.GetDisplayDecimalValue(MonthlyNetPay, CurrencySymbol);

    }
}