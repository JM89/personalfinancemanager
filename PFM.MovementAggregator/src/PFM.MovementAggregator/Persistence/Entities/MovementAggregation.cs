using System;
namespace PFM.MovementAggregator.Persistence.Entities
{
	public class MovementAggregation
	{
		public int BankAccountId { get; set; }
        public string MonthYearIdentifier { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public decimal AggregatedAmount { get; set; }
    }
}

