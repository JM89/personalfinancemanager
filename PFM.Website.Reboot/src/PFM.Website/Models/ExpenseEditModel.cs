using PFM.Website.Utils;
using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class ExpenseEditModel
	{
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        public DateTime DateExpense { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int ExpenseTypeId { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public string Description { get; set; }

        public int? AtmWithdrawId { get; set; }

        public int? TargetInternalAccountId { get; set; }

        public string DisplayedDateExpense => DateTimeFormatHelper.GetDisplayDateValue(this.DateExpense);

        public bool HasBeenAlreadyDebited { get; set; }

        public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }

        public int? GeneratedIncomeId { get; set; }
    }
}

