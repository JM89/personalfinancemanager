using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.Bank
{
    public class BankEditModel
    {
        public int Id { get; set; }

        [Required]
        [LocalizedDisplayName("BankName")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [LocalizedDisplayName("BankCountry")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        [LocalizedDisplayName("BankFavoriteBranch")]
        public BankBrandEditModel FavoriteBranch { get; set; }

        [LocalizedDisplayName("BankWebsite")]
        [MaxLength(200)]
        [Required]
        public string Website { get; set; }

        [LocalizedDisplayName("BankGeneralEnquiryPhoneNumber")]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Not a valid Phone number (00000000000)")]
        [Required]
        public string GeneralEnquiryPhoneNumber { get; set; }

        [LocalizedDisplayName("BankIconPath")]
        public string IconPath { get; set; }

        public string FileName { get
            {
                if (IconPath != null)
                {
                    var indexLastSlash = IconPath.LastIndexOf("/") + 1;
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
