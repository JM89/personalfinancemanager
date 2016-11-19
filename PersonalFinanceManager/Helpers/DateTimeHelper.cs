using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Helpers
{
    public class DateTimeHelper
    {
        public static Interval GetInterval(DateTime baseDate, DateTimeUnitEnums unit, int nb)
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
            DateTime startDate = new DateTime(baseStartDate.Year, baseStartDate.Month, 1);

            // Last date of the month
            DateTime baseEndDate = new DateTime(baseDate.Year, baseDate.Month, 1).AddDays(-1);

            return new Interval() {
                StartDate = startDate,
                EndDate = baseEndDate
            };
        }
    }
}