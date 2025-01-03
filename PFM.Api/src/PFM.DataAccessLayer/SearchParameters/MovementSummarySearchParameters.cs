﻿using System.Collections.Generic;

namespace PFM.DataAccessLayer.SearchParameters
{
	public class MovementSummarySearchParameters
	{
        public int BankAccountId { get; set; }

        public IList<string> MonthYearIdentifiers { get; set; } = null;

        public string OptionalType { get; set; } = null;

        public string OptionalCategory { get; set; } = null;

        public IList<string> ExcludedCategories { get; set; } = null;
	}
}

