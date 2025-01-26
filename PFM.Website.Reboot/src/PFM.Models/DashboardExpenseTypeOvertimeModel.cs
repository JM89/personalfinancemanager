namespace PFM.Models
{
	public class DashboardExpenseTypeOvertimeModel
	{
		/// <summary>
		/// Key: MonthYear, Value: Sum Amounts
		/// </summary>
		public IDictionary<string, decimal> Aggregates { get; }

		public DashboardExpenseTypeOvertimeModel(IDictionary<string, decimal> aggregates)
		{
			Aggregates = aggregates;
		}
    }
}

