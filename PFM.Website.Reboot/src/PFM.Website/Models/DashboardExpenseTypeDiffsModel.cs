namespace PFM.Website.Models
{
	public class DashboardExpenseTypeDiffsModel
	{
		public PagedModel<ExpenseTypeDiffsModel> PagedList { get; }

		public DashboardExpenseTypeDiffsModel(PagedModel<ExpenseTypeDiffsModel> pagedList)
        {
            PagedList = pagedList;
		}
    }

	public class ExpenseTypeDiffsModel
	{
		public string ExpenseTypeName { get; set; }

		public decimal Actual { get; set; }

        public decimal Expected { get; set; }

		public decimal ExpectedDiff => Actual - Expected;

        public decimal PreviousMonth { get; set; }

        public decimal PreviousMonthDiff => Actual - PreviousMonth;

        public decimal Average { get; set; }

        public decimal AverageDiff => Actual - Average;
    }
}

