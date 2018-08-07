using PFM.DTOs.Shared;

namespace PFM.DTOs.Country
{
    public class CountryList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "CountryCantBeDeleted";
    }
}
