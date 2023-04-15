namespace PersonalFinanceManager.Api.Contracts.ExpenseType
{
    public class ExpenseTypeDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GraphColor { get; set; }

        public bool ShowOnDashboard { get; set; }
    }
}