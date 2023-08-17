using System;
namespace PFM.Website.Models
{
	public class DashboardExpenseOvertimeModel
	{
		/// <summary>
		/// Key: MonthYear, Value: Actual sum, Expected sum (budget plan)
		/// </summary>
		public IDictionary<string, ExpenseOvertimeModel> Aggregates { get; }

		public DashboardExpenseOvertimeModel(IDictionary<string, ExpenseOvertimeModel> aggregates)
		{
			Aggregates = aggregates;
		}
    }

    public class ExpenseOvertimeModel
    {
        public decimal Actual { get; set; }

        /// <summary>
        /// For budget implementation
        /// </summary>
        public decimal Expected { get; set; }
    }
}

