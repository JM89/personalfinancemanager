using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Entities
{
    public class BudgetPlan : PersistedEntity
    {
         [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal ExpectedIncomes { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal ExpectedSavings { get; set; }
    }
}
