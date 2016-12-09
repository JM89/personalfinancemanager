using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;
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

        [LocalizedDisplayName("AccountBank")]
        public string BankName { get; set; }

        public string BankIconPath { get; set; }

        [LocalizedDisplayName("AccountCurrency")]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        [LocalizedDisplayName("AccountInitialBalance")]
        public decimal InitialBalance { get; set; }

        public string DisplayedInitialBalance
        {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(this.InitialBalance, this.CurrencySymbol);
            }
        }

        [LocalizedDisplayName("AccountCurrentBalance")]
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

        [LocalizedDisplayName("AccountIsSavingAccount")]
        public bool IsSavingAccount { get; set; }
    }
}
