using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class BankEditModel
	{
        public int? Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }

        public CountryModel Country { get; set; }

        [MaxLength(200)]
        [Required]
        public string Website { get; set; }

        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Not a valid Phone number (00000000000)")]
        [Required]
        public string GeneralEnquiryPhoneNumber { get; set; }

        public string IconPath { get; set; }

        public DisplayIcon DisplayIconFlags { get; set; }
    }

    [Flags]
    public enum DisplayIcon
    {
        None = 0,
        DisplayUploader = 1,
        DisplayExistingIcon = 2,
        DisplayIconPathPreview = 4,
        DisplayFilePreview = 8
    }
}

