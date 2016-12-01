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

        public Interval(DateTime startDate, DateTime endDate)
        {
            this.StartDate = new DateTime(startDate.Year, startDate.Month, 1); 

            this.EndDate = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
        } 

        public Interval(DateTime baseDate, DateTimeUnitEnums unit, int nb)
        {
            DateTime baseStartDate;

            if (unit == DateTimeUnitEnums.Months)
            {
                baseStartDate = baseDate.AddMonths(-nb);
            }
            else
            {
                baseStartDate = baseDate.AddYears(-nb);
            }

            // First date of the month
            this.StartDate = new DateTime(baseStartDate.Year, baseStartDate.Month, 1);

            // Last date of the month
            this.EndDate = new DateTime(baseDate.Year, baseDate.Month, 1).AddDays(-1);
        }

        public IDictionary<string, Interval> GetIntervalsByMonth()
        {
            DateTime iterator = StartDate;
            var names = new Dictionary<string, Interval>();
            while (iterator <= EndDate)
            {
                var nextMonth = iterator.AddMonths(1);
                names.Add(DateTimeHelper.GetMonthNameAndYear(iterator), new Interval(iterator, nextMonth.AddDays(-1)));
                iterator = nextMonth;
            }
            return names;
        }

        public int Count(DateTimeUnitEnums unit)
        {
            int nb = 0;
            DateTime iterator = StartDate;
            while (iterator <= EndDate)
            {
                nb++;
                if (unit == DateTimeUnitEnums.Months)
                {
                    iterator = iterator.AddMonths(1);
                }
                else
                {
                    iterator = iterator.AddYears(1);
                }
            }
            return nb;
        }

        public string GetSingleMonthName()
        {
            return DateTimeHelper.GetMonthNameAndYear(StartDate);
        }

        public bool IsBetween(DateTime date)
        {
            return date >= StartDate && date <= EndDate;
        }
    }
}