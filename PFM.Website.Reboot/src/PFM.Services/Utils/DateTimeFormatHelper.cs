using System.Globalization;

namespace PFM.Services.Utils;

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