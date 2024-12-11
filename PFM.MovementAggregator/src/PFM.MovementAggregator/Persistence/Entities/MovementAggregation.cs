using System;
namespace PFM.MovementAggregator.Persistence.Entities
{
	public class MovementAggregation
	{
		public int BankAccountId { get; set; }
        public string MonthYearIdentifier { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string Category { get; set; } = String.Empty;
        public decimal AggregatedAmount { get; set; }
    }
}

