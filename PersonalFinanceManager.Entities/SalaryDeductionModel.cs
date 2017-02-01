using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class SalaryDeductionModel : PersistedEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int SalaryId { get; set; }

        [ForeignKey("SalaryId")]
        public SalaryModel Salary { get; set; }
    }
}
