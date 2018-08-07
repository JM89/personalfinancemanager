using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.SearchParameters
{
    public class ExpenseGetListSearchParameters : ISearchParameters
    {
        public int? AccountId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ExpenditureTypeId { get; set; }

        public bool? ShowOnDashboard { get; set; }
    }
}
