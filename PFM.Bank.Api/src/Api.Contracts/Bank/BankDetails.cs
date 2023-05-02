using System;
using System.Collections.Generic;

namespace PFM.Api.Contracts.Bank
{
    public class BankDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public string Website { get; set; }

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
