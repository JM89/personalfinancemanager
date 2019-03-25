using PFM.Services.DTOs.Shared;

namespace PFM.Services.DTOs.Currency
{
    public class CurrencyList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "CurrencyCantBeDeleted";
    }
}
