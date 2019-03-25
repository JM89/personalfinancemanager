using System;
using PersonalFinanceManager.DTOs.Shared;

namespace PersonalFinanceManager.DTOs.Tax
{
    public class TaxList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string TaxType { get; set; }

        public string CurrencySymbol { get; set; }

        public string CountryName { get; set; }

        public decimal Amount { get; set; }

        public string FrequenceDescription { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "TaxCantBeDeleted";
    }
}