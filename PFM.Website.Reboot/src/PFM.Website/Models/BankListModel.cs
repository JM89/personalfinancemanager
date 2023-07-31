using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class BankListModel
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CountryName { get; set; }

        public string IconPath { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}

