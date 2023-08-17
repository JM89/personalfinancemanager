namespace PFM.Website.Models
{
	public class MovementSummarySearchParamModel
    {
		public int BankAccountId { get; }

        public IEnumerable<string> MonthYearIdentifiers { get; }

        public string? OptionalCategory { get; set; }

        public string? OptionalType { get; set; }

        public IList<string>? ExcludedCategories { get; set; }

        public MovementSummarySearchParamModel(int bankAccountId, IEnumerable<string> monthYearIdentifiers)
        {
            BankAccountId = bankAccountId;
            MonthYearIdentifiers = monthYearIdentifiers;
        }
    }
}

