using System.Collections.Generic;

namespace PFM.Api.Contracts.SearchParameters
{
	public class MovementSummarySearchParameters
	{
        public int BankAccountId { get; set; }

        public IList<string> MonthYearIdentifiers { get; set; }

        public string OptionalType { get; set; }

        public string OptionalCategory { get; set; }

        public IList<string> ExcludedCategories { get; set; }
    }
}

