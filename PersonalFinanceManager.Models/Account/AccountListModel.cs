using PersonalFinanceManager.Models.Helpers;
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

        public string BankIconPath { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        [DisplayName("Initial Balance")]
        public decimal InitialBalance { get; set; }

        public string DisplayedInitialBalance
        {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(this.InitialBalance, this.CurrencySymbol);
            }
        }

        [DisplayName("Current Balance")]
        public decimal CurrentBalance { get; set; }

        public string DisplayedCurrentBalance
        {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(this.CurrentBalance, this.CurrencySymbol);
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
        
        public bool IsFavorite { get; set; }
    }
}
