using System.ComponentModel.DataAnnotations;
using PFM.Utils;

namespace PFM.Models
{
	public class SavingEditModel
	{
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        public DateTime DateSaving { get; set; }

        [Required]
        [Range(0.00, 999999.99, ErrorMessage = "The field {0} must be positive.")]
        public decimal Amount { get; set; }

        public int? TargetInternalAccountId { get; set; }

        public string DisplayedDateSaving => DateTimeFormatHelper.GetDisplayDateValue(this.DateSaving);

        public string Description
        {
            get
            {
                return "Saving " + DisplayedDateSaving;
            }
        }

        public int? GeneratedIncomeId { get; set; }
    }
}

