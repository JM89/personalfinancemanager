using PFM.Utils.Helpers;
using System;
using System.Collections.Generic;

namespace PFM.DTOs.Bank
{
    public class BankDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public BankBranchDetails FavoriteBranch { get; set; }

        public string Website { get; set; }

        public string GeneralEnquiryPhoneNumber { get; set; }

        public string IconPath { get; set; }

        public string FileName { get
            {
                if (IconPath != null)
                {
                    var indexLastSlash = IconPath.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    var fileName = IconPath.Substring(indexLastSlash, IconPath.Length - indexLastSlash);
                    return fileName;
                }
                else
                {
                    return "";
                }
            }
        }

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
