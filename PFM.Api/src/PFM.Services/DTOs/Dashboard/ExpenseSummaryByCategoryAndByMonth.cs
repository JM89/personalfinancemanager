namespace PFM.Services.DTOs.Dashboard
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
