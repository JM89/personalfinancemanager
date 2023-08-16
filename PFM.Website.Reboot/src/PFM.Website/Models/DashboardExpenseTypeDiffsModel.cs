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

        public decimal PreviousMonth { get; set; }

        public decimal Average { get; set; }
    }
}

