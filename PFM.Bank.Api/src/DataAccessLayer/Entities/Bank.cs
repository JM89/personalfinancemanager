using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PFM.DataAccessLayer.Entities
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
