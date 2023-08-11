using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
    public class IncomeEditModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateIncome { get; set; }
    }
}

