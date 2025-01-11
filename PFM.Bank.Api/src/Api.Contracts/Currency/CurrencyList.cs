using PFM.Bank.Api.Contracts.Shared;

// ReSharper disable once CheckNamespace
namespace PFM.Bank.Api.Contracts.Currency
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
