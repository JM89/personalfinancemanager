using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Helpers
{
    public class DateTimeHelper
    {
        public static string GetMonthNameAndYear(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month) + " " + date.Year;
        }

        public static string GetStringFormat(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}