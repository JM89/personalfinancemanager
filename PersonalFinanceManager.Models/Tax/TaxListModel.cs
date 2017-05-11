using System;
using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Models.Tax
{
    public class TaxListModel : ICanBeDeleted
    {
        [LocalizedDisplayName("TaxId")]
        public int Id { get; set; }

        [LocalizedDisplayName("TaxDescription")]
        public string Description { get; set; }

        [LocalizedDisplayName("TaxCode")]
        public string Code { get; set; }

        [LocalizedDisplayName("TaxStartDate")]
        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        [LocalizedDisplayName("TaxEndDate")]
        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        [LocalizedDisplayName("TaxType")]
        public string TaxType { get; set; }

        public string CurrencySymbol { get; set; }

        [LocalizedDisplayName("TaxCountry")]
        public string CountryName { get; set; }

        [LocalizedDisplayName("TaxAmount")]
        public decimal Amount { get; set; }

        public string DisplayedAmount => DecimalFormatHelper.GetDisplayDecimalValue(Amount, CurrencySymbol);

        [LocalizedDisplayName("TaxFrequence")]
        public string FrequenceDescription { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "TaxCantBeDeleted";
    }
}