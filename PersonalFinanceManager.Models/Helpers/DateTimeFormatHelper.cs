using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Helpers
{
    public static class DateTimeFormatHelper
    {
        public static string GetDisplayDateValue(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string GetDisplayDateValue(DateTime? date)
        {
            if (date.HasValue)
            {
                return GetDisplayDateValue(date.Value);
            }
            return string.Empty;
        }
    }
}
