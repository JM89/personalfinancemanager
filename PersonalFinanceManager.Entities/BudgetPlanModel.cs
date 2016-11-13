using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class BudgetPlanModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //public IList<BudgetByExpenditureTypeModel> PlannedExpenditures { get; set; }

        public IList<CommentModel> Comments { get; set; }

        public IList<ChallengeModel> ToDos { get; set; }

        public IList<SavingsPlanModel> Savings { get; set; }
    }
}
