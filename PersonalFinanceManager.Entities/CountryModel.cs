using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class CountryModel : PersistedEntity
    {
        public string Name { get; set; }
    }
}