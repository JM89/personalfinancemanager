using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class HistoricMovementModel : PersistedEntity
    {
        public int? AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountModel Account { get; set; }

        public int? AtmWithdrawId { get; set; }

        [ForeignKey("AtmWithdrawId")]
        public AtmWithdrawModel AtmWithdraw { get; set; }

        public int? InternalAccountId { get; set; }

        [ForeignKey("InternalAccountId")]
        public AtmWithdrawModel InternalAccount { get; set; }

        public decimal SourceOldAmount { get; set; }

        public decimal DestinationOldAmount { get; set; }

        public decimal SourceNewAmount { get; set; }

        public decimal DestinationNewAmount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Cost { get; set; }
    }
}