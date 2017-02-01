using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class SalaryModel : PersistedEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public decimal YearlySalary { get; set; }

        [Required]
        public decimal MonthlyGrossPay { get; set; }

        [Required]
        public decimal MonthlyNetPay { get; set; }

        public string TaxCode { get; set; }

        public decimal TaxPercentage { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public CurrencyModel Currency { get; set; }
    }
}
