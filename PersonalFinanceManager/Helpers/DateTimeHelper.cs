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
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month) + " " + date.ToString("yy");
        }

        public static string GetStringFormat(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}