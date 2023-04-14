using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Utils.Helpers
{
    public static class DateTimeFormatHelper
    {
        public static string GetDisplayDateValue(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string GetDisplayDateValue(DateTime? date)
        {
            return date.HasValue ? GetDisplayDateValue(date.Value) : string.Empty;
        }

        public static string GetMonthNameAndYear(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month) + " " + date.ToString("yy");
        }

        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}
