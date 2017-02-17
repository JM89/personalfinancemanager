using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class PensionModel : PersistedEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Website { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public decimal ContributionPercentage { get; set; }

        [Required]
        public decimal CompanyContributionPercentage { get; set; }

        [Required]
        public decimal CurrentPot { get; set; }

        [Required]
        public decimal Interest { get; set; }

        [Required]
        public string UserId { get; set; }
        
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
