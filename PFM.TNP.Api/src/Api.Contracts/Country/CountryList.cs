using Api.Contracts.Shared;

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
