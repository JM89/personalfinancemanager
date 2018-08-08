using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.UserProfile
{
    public class UserProfileEditModel
    {
        public int Id { get; set; }

        public string User_Id { get; set; }

        [LocalizedDisplayName("UserProfileFirstName")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [LocalizedDisplayName("UserProfileLastName")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [LocalizedDisplayName("UserProfileYearlyWages")]
        [Required]
        public decimal YearlyWages { get; set; }

        [LocalizedDisplayName("UserProfileSourceWages")]
        [Required]
        [MaxLength(50)]
        public string SourceWages { get; set; }
    } 
}
