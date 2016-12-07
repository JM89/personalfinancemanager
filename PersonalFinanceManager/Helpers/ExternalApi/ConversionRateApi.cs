using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace PersonalFinanceManager.Helpers.ExternalApi
{
    public static class ConversionRateApi
    {
        private static string BaseUrl = "http://api.fixer.io/latest?base={0}&symbols={1}";

        public static ConversionRateResult GetRate(string baseCurrency, string targetCurrency)
        {
            var conversionRate = new ConversionRateResult();

            try
            {
                using (var client = new HttpClient())
                {
                    var result = client.GetAsync(string.Format(BaseUrl, baseCurrency, targetCurrency)).Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonContent = result.Content.ReadAsStringAsync().Result;
                        var jObject = JObject.Parse(jsonContent);
                        conversionRate.BaseCurrency = baseCurrency;
                        conversionRate.ConversionRate = new ConversionRateCurrencyResult()
                        {
                            Currency = targetCurrency,
                            ConversionRate = (decimal)jObject["rates"].ToArray().First()
                        };
                    }
                }
            }
            catch(Exception)
            {
                // Validation is done through the object (IsValid), we don't want the API down => PFM down
            }

            return conversionRate;
        }
    }
}