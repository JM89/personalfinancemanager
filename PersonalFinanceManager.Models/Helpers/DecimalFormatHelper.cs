using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Helpers
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
    }
}
