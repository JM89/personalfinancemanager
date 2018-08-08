using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Helpers.ExternalApi
{
    public class ConversionRateResult
    {
        public string BaseCurrency { get; set; }

        public ConversionRateCurrencyResult ConversionRate { get; set; }

        public bool Valid => !string.IsNullOrEmpty(BaseCurrency) && ConversionRate != null;
    }

    public class ConversionRateCurrencyResult
    {
        public string Currency { get; set; }

        public decimal ConversionRate { get; set; }
    }
}