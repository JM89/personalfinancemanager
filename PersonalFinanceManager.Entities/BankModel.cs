using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class BankModel : PersistedEntity
    {
        public string Name { get; set; }

        public string IconPath { get; set; }

        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public CountryModel Country { get; set; }

        public string Website { get; set; }

        public string GeneralEnquiryPhoneNumber { get; set; }
    }
}