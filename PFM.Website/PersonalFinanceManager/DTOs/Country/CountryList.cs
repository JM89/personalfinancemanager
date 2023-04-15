using PersonalFinanceManager.Api.Contracts.Shared;

namespace PersonalFinanceManager.Api.Contracts.Country
{
    public class CountryList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "CountryCantBeDeleted";
    }
}
