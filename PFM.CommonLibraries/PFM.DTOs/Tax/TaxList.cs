using System;
using PFM.DTOs.Shared;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Tax
{
    public class TaxList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public string TaxType { get; set; }

        public string CurrencySymbol { get; set; }

        public string CountryName { get; set; }

        public decimal Amount { get; set; }

        public string DisplayedAmount => DecimalFormatHelper.GetDisplayDecimalValue(Amount, CurrencySymbol);

        public string FrequenceDescription { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "TaxCantBeDeleted";
    }
}