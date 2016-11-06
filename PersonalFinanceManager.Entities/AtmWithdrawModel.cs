using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class AtmWithdrawModel
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
        public decimal InitialAmount { get; set; }

        [Required]
        public decimal CurrentAmount { get; set; }

        public bool IsClosed { get; set; }

        [DisplayName("Already Debited")]
        public bool HasBeenAlreadyDebited { get; set; }
    }
}