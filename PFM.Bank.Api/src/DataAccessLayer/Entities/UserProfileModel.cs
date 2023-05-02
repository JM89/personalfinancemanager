using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.DataAccessLayer.Entities
{
    public class UserProfile : PersistedEntity
    {
        [Required]
        public string User_Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal YearlyWages { get; set; }

        [Required]
        public string SourceWages { get; set; }
    }
}