using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class BudgetByExpenditureTypeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ExpenditureTypeId { get; set; }

        [ForeignKey("ExpenditureTypeId")]
        public ExpenditureTypeModel ExpenditureType { get; set; }

        public decimal Budget { get; set; }

        public decimal? Actual { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountModel Account { get; set; }

        [Required]
        public int BudgetPlanId { get; set; }

        [ForeignKey("BudgetPlanId")]
        public BudgetPlanModel BudgetPlan { get; set; }

    }
}
