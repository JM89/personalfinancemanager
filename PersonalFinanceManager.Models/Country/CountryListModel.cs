
using PersonalFinanceManager.Models.Resources;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManager.Models.Country
{
    public class CountryListModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("CountryName")]
        [Required]
        public string Name { get; set; }
    }
}
