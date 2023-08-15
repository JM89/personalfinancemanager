namespace PFM.Website.Models
{
	public class ExpensePagedListModel
	{
		public IList<ExpenseListModel> Expenses { get; }
		public int Count { get; }

		public ExpensePagedListModel(IList<ExpenseListModel> expenses, int count)
		{
			this.Expenses = expenses;
			this.Count = count;
		}
	}
}

