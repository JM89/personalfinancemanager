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
        public string Name { get; set; }

        [Required]
        [LocalizedDisplayName("BankCountry")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        [LocalizedDisplayName("BankUploadImage")]
        public string FileName { get; set; }

        [LocalizedDisplayName("BankIconPathPreview")]
        public string UrlPreview { get; set; }

        public string ErrorPreview { get; set; }

        public int AttemptNumber { get; set; }
    } 
}
