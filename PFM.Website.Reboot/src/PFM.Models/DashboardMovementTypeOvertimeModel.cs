namespace PFM.Models
{
	public class DashboardMovementTypeOvertimeModel
	{
		/// <summary>
		/// Key: MonthYear, Value: Expense sum, Incomes sum, Savings sum
		/// </summary>
		public IDictionary<string, MovementTypeOvertimeModel> Aggregates { get; }

		public DashboardMovementTypeOvertimeModel(IDictionary<string, MovementTypeOvertimeModel> aggregates)
		{
			Aggregates = aggregates;
		}
    }

	public class MovementTypeOvertimeModel
	{
        public decimal ExpensesAmount { get; set; }

        public decimal IncomesAmount { get; set; }

        public decimal SavingsAmount { get; set; }
	}
}

