using PFM.Bank.Api.Contracts.Shared;

// ReSharper disable once CheckNamespace
namespace PFM.Bank.Api.Contracts.Country
{
    public class CountryList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "CountryCantBeDeleted";
    }
}
