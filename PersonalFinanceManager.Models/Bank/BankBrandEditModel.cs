using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Bank
{
    public class BankBrandEditModel
    {
        public int Id { get; set; }

        public int BankId { get; set; }

        [LocalizedDisplayName("BankBranchName")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [LocalizedDisplayName("BankBranchAddressLine1")]
        [Required]
        [MaxLength(255)]
        public string AddressLine1 { get; set; }

        [LocalizedDisplayName("BankBranchAddressLine2")]
        [MaxLength(255)]
        public string AddressLine2 { get; set; }

        [LocalizedDisplayName("BankBranchPostCode")]
        [Required]
        [MaxLength(10)]
        public string PostCode { get; set; }

        [LocalizedDisplayName("BankBranchCity")]
        [Required]
        [MaxLength(255)]
        public string City { get; set; }

        [LocalizedDisplayName("BankBranchPhoneNumber")]
        [Required]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Not a valid Phone number (00000000000)")]
        public string PhoneNumber { get; set; }
    }
}
