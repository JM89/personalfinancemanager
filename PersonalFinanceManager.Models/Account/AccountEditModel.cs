using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.Account
{
    public class AccountEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Bank")]
        public int BankId { get; set; }

        [Required]
        [DisplayName("Currency")]
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        [Required]
        [DisplayName("Initial Balance")]
        public decimal InitialBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        public IList<SelectListItem> AvailableBanks { get; set; }

        public string BankWebsite { get; set; }

        public string BankIconPath { get; set; }

        public string BankBranchName { get; set; }

        public string BankBranchAddressLine1 { get; set; }

        public string BankBranchAddressLine2 { get; set; }

        public string BankBranchPostCode { get; set; }

        public string BankBranchCity { get; set; }

        public string BankBranchPhoneNumber { get; set; }
    }
}