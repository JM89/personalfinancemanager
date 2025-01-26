using System.ComponentModel.DataAnnotations;

namespace PFM.Models
{
	public class AtmWithdrawEditModel
	{
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        public DateTime DateExpense { get; set; }

        public string DisplayedDateExpense => this.DateExpense.ToString("yyyy-MM-dd");

        [Required]
        public decimal InitialAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public bool HasBeenAlreadyDebited { get; set; }
    }
}

