using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Bank : PersistedEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string IconPath { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public string Website { get; set; }

        public string GeneralEnquiryPhoneNumber { get; set; }
    }
}
