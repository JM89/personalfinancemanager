namespace PFM.Models
{
	public class DashboardExpenseTypeSummaryModel
	{
        /// <summary>
        /// Key: Expense Type Name, Value: Actual sum, Expected sum (budget plan)
        /// </summary>
        public IDictionary<string, decimal> Aggregates { get; }

        public DashboardExpenseTypeSummaryModel(IDictionary<string, decimal> aggregates)
        {
            Aggregates = aggregates;
        }
    }
}

