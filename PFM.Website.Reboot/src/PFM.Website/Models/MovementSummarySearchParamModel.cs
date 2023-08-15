namespace PFM.Website.Models
{
	public class MovementSummarySearchParamModel
    {
		public int BankAccountId { get; set; }

        public IList<string> MonthYearIdentifiers { get; set; }

        public string? OptionalCategory { get; set; }

        public string? OptionalType { get; set; }
    }
}

