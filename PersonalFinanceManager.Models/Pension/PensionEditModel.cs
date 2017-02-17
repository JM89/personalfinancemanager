using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;

namespace PersonalFinanceManager.Models.Pension
{
    public class PensionEditModel
    {
        [Required]
        [LocalizedDisplayName("PensionId")]
        public int Id { get; set; }

        [Required]
        [LocalizedDisplayName("PensionDescription")]
        public string Description { get; set; }

        [Required]
        [LocalizedDisplayName("PensionWebsite")]
        public string Website { get; set; }

        [Required]
        [LocalizedDisplayName("PensionStartDate")]
        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        [LocalizedDisplayName("PensionEndDate")]
        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        [Required]
        [LocalizedDisplayName("PensionContributionPercentage")]
        public decimal ContributionPercentage { get; set; }

        [Required]
        [LocalizedDisplayName("PensionCompanyContributionPercentage")]
        public decimal CompanyContributionPercentage { get; set; }

        [Required]
        [LocalizedDisplayName("PensionCurrentPot")]
        public decimal CurrentPot { get; set; }

        [Required]
        [LocalizedDisplayName("PensionInterest")]
        public decimal Interest { get; set; }

        public string UserId { get; set; }

        [Required]
        [LocalizedDisplayName("PensionCurrency")]
        public int CurrencyId { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [Required]
        [LocalizedDisplayName("PensionCountry")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }
    }
}