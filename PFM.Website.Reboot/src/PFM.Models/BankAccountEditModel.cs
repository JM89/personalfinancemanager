using System.ComponentModel.DataAnnotations;
using PFM.Utils;

namespace PFM.Models
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

