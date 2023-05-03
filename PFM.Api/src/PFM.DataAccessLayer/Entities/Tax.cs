using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.DataAccessLayer.Entities
{
    public class Tax : PersistedEntity
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
        public TaxType TaxType { get; set; }
        
        [Required]
        public int FrequenceOptionId { get; set; }

        [ForeignKey("FrequenceOptionId")]
        public FrequenceOption FrequenceOption { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Amount { get; set; }

        public int? Frequence { get; set; }
        
        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public int CountryId { get; set; }
    }
}
