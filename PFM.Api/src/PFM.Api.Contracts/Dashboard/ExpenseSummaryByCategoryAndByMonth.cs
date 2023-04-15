namespace PFM.Api.Contracts.Dashboard
{
    public class ExpenseSummaryByCategoryAndByMonth
    {
        public decimal CategoryExpenses { get; set; }

        public ExpenseSummaryByCategoryAndByMonth(decimal categoryExpenses)
        {
            this.CategoryExpenses = categoryExpenses;
        }
    }
}
