using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class SavingModel : PersistedEntity
    {
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountModel Account { get; set; }

        [Required]
        public DateTime DateSaving { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int TargetInternalAccountId { get; set; }

        [ForeignKey("TargetInternalAccountId")]
        public AccountModel TargetInternalAccount { get; set; }
    }
}