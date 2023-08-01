
using PFM.Website.Utils;
using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class BankAccountEditModel
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int BankId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        [Required]
        public decimal InitialBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public string DisplayCurrentBalance
            => DecimalFormatHelper.GetDisplayDecimalValue(CurrentBalance, CurrencySymbol);

        public bool IsSavingAccount { get; set; }

    }
}

