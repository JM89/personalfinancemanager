using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Entities
{
    public class BudgetByExpenseType : PersistedEntity
    {
        [Required]
        public int ExpenseTypeId { get; set; }

        [ForeignKey("ExpenseTypeId")]
        public ExpenseType ExpenseType { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Budget { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal? Actual { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int BudgetPlanId { get; set; }

        [ForeignKey("BudgetPlanId")]
        public BudgetPlan BudgetPlan { get; set; }

    }
}
