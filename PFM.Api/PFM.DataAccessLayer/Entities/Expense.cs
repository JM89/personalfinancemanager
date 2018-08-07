using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFM.DataAccessLayer.Entities
{
    public class Expense : PersistedEntity
    {
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [Required]
        public DateTime DateExpense { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Cost { get; set; }

        [Required]
        public int ExpenseTypeId { get; set; }

        [ForeignKey("ExpenseTypeId")]
        public ExpenseType ExpenseType { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public string Description { get; set; }
       
        [DisplayName("Already Debited")]
        public bool HasBeenAlreadyDebited { get; set; }

        public int? AtmWithdrawId { get; set; }

        [ForeignKey("AtmWithdrawId")]
        public AtmWithdraw AtmWithdraw { get; set; }

        public int? TargetInternalAccountId { get; set; }

        [ForeignKey("TargetInternalAccountId")]
        public Account TargetInternalAccount { get; set; }

        public int? GeneratedIncomeId { get; set; }
    }
   
}