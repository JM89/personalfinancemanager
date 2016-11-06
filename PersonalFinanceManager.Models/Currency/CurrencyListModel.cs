using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Currency
{
    public class CurrencyListModel : ICanBeDeleted
    {
        public int Id { get; set; }

        [LocalizedDisplayName("CurrencyName")]
        public string Name { get; set; }

        [LocalizedDisplayName("CurrencySymbol")]
        public string Symbol { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName
        {
            get
            {
                return "CurrencyCantBeDeleted";
            }
            set
            {

            }
        }
    }
}
