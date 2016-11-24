
using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManager.Models.Country
{
    public class CountryListModel : ICanBeDeleted
    {
        public int Id { get; set; }

        [LocalizedDisplayName("CountryName")]
        [Required]
        public string Name { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName
        {
            get
            {
                return "CountryCantBeDeleted";
            }
            set
            {

            }
        }
    }
}
