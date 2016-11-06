using PersonalFinanceManager.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Account
{
    public class AccountListModel: ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Bank")]
        public string BankName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        [DisplayName("Initial Balance")]
        public decimal InitialBalance { get; set; }

        public string DisplayedInitialBalance
        {
            get
            {
                return this.CurrencySymbol + this.InitialBalance;
            }
        }

        [DisplayName("Current Balance")]
        public decimal CurrentBalance { get; set; }

        public string DisplayedCurrentBalance
        {
            get
            {
                return this.CurrencySymbol + this.CurrentBalance;
            }
        }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName
        {
            get
            {
                return "AccountCantBeDeleted";
            }
            set
            {

            }
        }
    }
}
