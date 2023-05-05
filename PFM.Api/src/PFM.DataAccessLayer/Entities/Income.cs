using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.DataAccessLayer.Entities
{
    public class Income : PersistedEntity
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Cost { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateIncome { get; set; }
    }
}