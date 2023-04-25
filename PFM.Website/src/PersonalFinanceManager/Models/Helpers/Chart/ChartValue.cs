using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Models.Helpers.Chart
{
    public class ChartValue
    {
        public string Value { get; set; }

        public string Color { get; set; }

        public ChartValue(string value)
        {
            this.Value = value;
        }
    }
}