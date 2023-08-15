namespace PFM.Website.ExternalServices.Contracts
{
	public class MovementSummarySearchParams
	{
		public int BankAccountId { get; set; }

        public IList<string> MonthYearIdentifiers { get; set; }

        public string? OptionalType { get; set; }

        public string? OptionalCategory { get; set; }
    }
}

