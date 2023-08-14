namespace PFM.Website.Utils
{
	public static class MonthYearHelper
	{
        private static Dictionary<int, string> _shortMonthNames = new Dictionary<int, string>()
        {
            { 1, "Jan" },
            { 2, "Feb" },
            { 3, "Mar" },
            { 4, "Apr" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "Aug" },
            { 9, "Sept" },
            { 10, "Oct" },
            { 11, "Nov" },
            { 12, "Dec" }
        };

        public static IEnumerable<string> GetXLastMonths(int x, bool includeCurrent = true, bool uiFriendly = true)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var startMonth = today.AddMonths(-x);
            var endMonth = today.AddMonths(-1);

            if (includeCurrent)
            {
                startMonth = startMonth.AddMonths(1);
                endMonth = endMonth.AddMonths(1);
            }

            var labels = new List<string>();
            while (startMonth <= endMonth)
            {
                var asterisk = includeCurrent && startMonth.Month == today.Month ? "*" : "";
                var label = uiFriendly ?
                    $"{_shortMonthNames[startMonth.Month]} {startMonth.Year.ToString().Substring(2)}{asterisk}":
                    $"{startMonth.Year}{startMonth.Month.ToString().PadLeft(2, '0')}";

                labels.Add(label);
                startMonth = startMonth.AddMonths(1);
            }

            return labels;
        }
    }
}

