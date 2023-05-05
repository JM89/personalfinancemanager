using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFM.DataAccessLayer.Entities
{
    public class AtmWithdraw : PersistedEntity
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public DateTime DateExpense { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal InitialAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal CurrentAmount { get; set; }

        public bool IsClosed { get; set; }

        [DisplayName("Already Debited")]
        public bool HasBeenAlreadyDebited { get; set; }
    }
}