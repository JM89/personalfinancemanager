using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Home
{
    public class ConversionRateModel
    {
        public string BaseCurrencySymbol { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal CurrencyConversionRate { get; set; }
        
        public string DisplayCurrencyConversionRate
        {
            get
            {
                return CurrencySymbol + CurrencyConversionRate.ToString("0.00");
            }
        }
    }
}
