using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class ExpenseTypeModel
	{
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string GraphColor { get; set; }

        [Required]
        public bool ShowOnDashboard { get; set; }
    }
}

