using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.SearchParameters
{
    public class ExpenditureGetListSearchParameters
    {
        public string UserId { get; set; }

        public int? AccountId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ExpenditureTypeId { get; set; }

        public bool? ShowOnDashboard { get; set; }
    }
}
