using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DTOs.SearchParameters
{
    public class ExpenseGetListSearchParameters
    {
        public string UserId { get; set; }

        public int? AccountId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ExpenseTypeId { get; set; }

        public bool? ShowOnDashboard { get; set; }
    }
}
