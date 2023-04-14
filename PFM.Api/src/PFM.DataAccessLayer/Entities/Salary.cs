using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Entities
{
    public class Salary : PersistedEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal YearlySalary { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal MonthlyGrossPay { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal MonthlyNetPay { get; set; }

        [Required]
        public int TaxId { get; set; }

        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [Required]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
