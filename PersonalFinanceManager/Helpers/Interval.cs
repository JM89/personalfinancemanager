using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Helpers
{
    public class Interval
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IDictionary<string, Interval> GetIntervalsByMonth()
        {
            DateTime iterator = StartDate;
            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            var names = new Dictionary<string, Interval>();
            while (iterator <= EndDate)
            {
                var nextMonth = iterator.AddMonths(1);
                names.Add(dateTimeFormat.GetMonthName(iterator.Month) + " " + iterator.Year, new Interval() { StartDate = iterator, EndDate = nextMonth.AddDays(-1)});
                iterator = nextMonth;
            }
            return names;
        }

        public bool IsBetween(DateTime date)
        {
            return date >= StartDate && date <= EndDate;
        }
    }
}