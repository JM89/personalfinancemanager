using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils.Helpers
{
    public static class DecimalFormatHelper
    {
        public static string GetDisplayDecimalValue(decimal value, string currency)
        {
            return currency + value.ToString("0.00");
        }

        public static string GetDisplayDecimalValue(decimal? value, string currency)
        {
            if (value.HasValue)
            {
                return GetDisplayDecimalValue(value.Value, currency);
            }
            return string.Empty;
        }

        public static string GetSignedCurrency(decimal value, string currency)
        {
            var absoluteValue = value;
            var sign = "+";
            if (value < 0)
            { 
                absoluteValue = Math.Abs(value);
                sign = "-";
            }
            return sign + GetDisplayDecimalValue(absoluteValue, currency);
        }
    }
}
