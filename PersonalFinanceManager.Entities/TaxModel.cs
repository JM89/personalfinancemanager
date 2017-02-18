using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class TaxModel : PersistedEntity
    {
        [Required]
        public string Description { get; set; }

        public string Code { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TaxTypeId { get; set; }

        [ForeignKey("TaxTypeId")]
        public TaxTypeModel TaxType { get; set; }
        
        [Required]
        public int FrequenceOptionId { get; set; }

        [ForeignKey("FrequenceOptionId")]
        public FrequenceOptionModel FrequenceOption { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int? Frequence { get; set; }
        
        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public CurrencyModel Currency { get; set; }

        [Required]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public CountryModel Country { get; set; }
    }
}
