using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Bank
{
    public class BankBrandEditModel
    {
        [LocalizedDisplayName("BankBranchName")]
        public string Name { get; set; }

        [LocalizedDisplayName("BankBranchAddressLine1")]
        public string AddressLine1 { get; set; }

        [LocalizedDisplayName("BankBranchAddressLine2")]
        public string AddressLine2 { get; set; }

        [LocalizedDisplayName("BankBranchPostCode")]
        public string PostCode { get; set; }

        [LocalizedDisplayName("BankBranchCity")]
        public string City { get; set; }

        [LocalizedDisplayName("BankBranchPhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
