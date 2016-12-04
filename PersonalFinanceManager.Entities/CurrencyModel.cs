using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class CurrencyModel : PersistedEntity
    {
        public string Name { get; set; }

        public string Symbol { get; set; }
    }
}