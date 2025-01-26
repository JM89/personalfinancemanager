using System.ComponentModel.DataAnnotations;

namespace PFM.Models
{
    public class IncomeEditModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        [Range(0.00, 999999.99, ErrorMessage = "The field {0} must be positive.")]
        public decimal Cost { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public DateTime DateIncome { get; set; }
    }
}

