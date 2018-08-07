using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.DataAccessLayer.Entities
{
    public class Pension : PersistedEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Website { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal ContributionPercentage { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal CompanyContributionPercentage { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal CurrentPot { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Interest { get; set; }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [Required]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
