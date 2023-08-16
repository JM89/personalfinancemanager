using System;

namespace PFM.DataAccessLayer.SearchParameters
{
    public class ExpenseGetListSearchParameters : ISearchParameters
    {
        public int? AccountId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ExpenseTypeId { get; set; }

        public bool? ShowOnDashboard { get; set; }
    }
}
