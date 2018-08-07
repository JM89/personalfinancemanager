using System;
using System.Collections.Generic;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Tax
{
    public class TaxDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public string UserId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public int FrequenceOptionId { get; set; }

        public IList<SelectListItem> AvailableFrequenceOptions { get; set; }

        public int? Frequence { get; set; }

        public int TaxTypeId { get; set; }

        public IList<SelectListItem> AvailableTaxTypes { get; set; }
    }
}