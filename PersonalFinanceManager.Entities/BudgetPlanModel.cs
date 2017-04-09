using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class BudgetPlanModel : PersistedEntity
    {
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public IList<CommentModel> Comments { get; set; }

        public IList<ChallengeModel> ToDos { get; set; }

        public decimal ExpectedIncomes { get; set; }

        public decimal ExpectedSavings { get; set; }
    }
}
