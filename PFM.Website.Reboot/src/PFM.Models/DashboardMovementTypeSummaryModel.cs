namespace PFM.Models
{
	public class DashboardMovementTypeSummaryModel
    {
        public decimal TotalExpensesCurrentMonth { get; set; }

        public decimal AverageExpensesOver12Months { get; set; }

        public decimal AverageIncomesOver12Months { get; set; }

        public decimal AverageSavingsOver12Months { get; set; }

        public DashboardMovementTypeSummaryModel(
            decimal totalExpensesCurrentMonth,
            decimal averageExpensesOver12Months,
            decimal averageIncomesOver12Months,
            decimal averageSavingsOver12Months
        )
        {
            TotalExpensesCurrentMonth = totalExpensesCurrentMonth;
            AverageExpensesOver12Months = averageExpensesOver12Months;
            AverageIncomesOver12Months = averageIncomesOver12Months;
            AverageSavingsOver12Months = averageSavingsOver12Months;
        }
    }
}

