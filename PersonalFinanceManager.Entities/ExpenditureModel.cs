using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace PersonalFinanceManager.Entities
{
    public class ExpenditureModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountModel Account { get; set; }

        [Required]
        public DateTime DateExpenditure { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int TypeExpenditureId { get; set; }

        [ForeignKey("TypeExpenditureId")]
        public ExpenditureTypeModel TypeExpenditure { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethodModel PaymentMethod { get; set; }

        [Required]
        public string Description { get; set; }
       
        [DisplayName("Already Debited")]
        public bool HasBeenAlreadyDebited { get; set; }

        public int? AtmWithdrawId { get; set; }

        [ForeignKey("AtmWithdrawId")]
        public AtmWithdrawModel AtmWithdraw { get; set; }

        public int? TargetInternalAccountId { get; set; }

        [ForeignKey("TargetInternalAccountId")]
        public AccountModel TargetInternalAccount { get; set; }
    }
   
}