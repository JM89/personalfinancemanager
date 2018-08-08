using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Resources;
using PFM.Utils.Helpers;

namespace PersonalFinanceManager.Models.Tax
{
    public class TaxEditModel
    {
        [Required]
        [LocalizedDisplayName("TaxId")]
        public int Id { get; set; }

        [Required]
        [LocalizedDisplayName("TaxDescription")]
        public string Description { get; set; }

        [LocalizedDisplayName("TaxCode")]
        public string Code { get; set; }

        [Required]
        [LocalizedDisplayName("TaxStartDate")]
        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);
        [LocalizedDisplayName("TaxEndDate")]
        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public string UserId { get; set; }

        [Required]
        [LocalizedDisplayName("TaxCurrency")]
        public int CurrencyId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [Required]
        [LocalizedDisplayName("TaxCountry")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        [Required]
        [LocalizedDisplayName("TaxFrequenceOption")]
        public int FrequenceOptionId { get; set; }

        public IList<SelectListItem> AvailableFrequenceOptions { get; set; }

        [LocalizedDisplayName("TaxFrequence")]
        public int? Frequence { get; set; }

        [Required]
        [LocalizedDisplayName("TaxType")]
        public int TaxTypeId { get; set; }

        public IList<SelectListItem> AvailableTaxTypes { get; set; }
    }
}