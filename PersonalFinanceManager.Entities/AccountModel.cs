using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class AccountModel
    {
        [Required(ErrorMessage = "The Account field is required.")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int BankId { get; set; }

        [ForeignKey("BankId")]
        public BankModel Bank { get; set; }

        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public CurrencyModel Currency { get; set; }

        public string User_Id { get; set; }

        [DisplayName("Initial Balance")]
        public decimal InitialBalance { get; set; }
        
        [NotMapped]
        public string DisplayedInitialBalance
        {
            get
            {
                return this.Currency.Symbol + this.InitialBalance;
            }
        }

        [DisplayName("Current Balance")]
        public decimal CurrentBalance { get; set; }

        [NotMapped]
        public string DisplayedCurrentBalance
        {
            get
            {
                return this.Currency.Symbol + this.CurrentBalance;
            }
        }
    }
}