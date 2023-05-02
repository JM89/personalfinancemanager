using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFM.DataAccessLayer.Entities
{
    public class Saving : PersistedEntity
    {
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [Required]
        public DateTime DateSaving { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Amount { get; set; }

        [Required]
        public int TargetInternalAccountId { get; set; }

        [ForeignKey("TargetInternalAccountId")]
        public Account TargetInternalAccount { get; set; }
        
        public int? GeneratedIncomeId { get; set; }

        [ForeignKey("GeneratedIncomeId")]
        public Income GeneratedIncome { get; set; }
    }
}