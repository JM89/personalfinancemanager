using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.DataAccessLayer.Entities
{
    public class Account : PersistedEntity
    {
        public string Name { get; set; }

        public int BankId { get; set; }

        [ForeignKey("BankId")]
        public Bank Bank { get; set; }

        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        public string User_Id { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal InitialBalance { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal CurrentBalance { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsSavingAccount { get; set; }
    }
}