namespace PFM.Api.Contracts.MovementSummary
{
    public class MovementSummary
    {
        public int BankAccountId { get; set; }

        public string MonthYearIdentifier { get; set; }

        public string Type { get; set; }

        public string Category { get; set; }

        public decimal AggregatedAmount { get; set; }
    }
}

