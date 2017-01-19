using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class IncomeModel : PersistedEntity
    {
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountModel Account { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateIncome { get; set; }
    }
}