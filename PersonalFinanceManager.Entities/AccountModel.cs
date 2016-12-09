using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class AccountModel : PersistedEntity
    {
        public string Name { get; set; }

        public int BankId { get; set; }

        [ForeignKey("BankId")]
        public BankModel Bank { get; set; }

        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public CurrencyModel Currency { get; set; }

        public string User_Id { get; set; }

        public decimal InitialBalance { get; set; }
        
        public decimal CurrentBalance { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsSavingAccount { get; set; }
    }
}