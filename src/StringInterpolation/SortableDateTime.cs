using System.Globalization;

namespace StringInterpolation;

public static class SortableDateTime
{
    /// <summary>
    /// Overrides DateTimeFormat to be sortable, yyyy-MM-dd HH:mm:ss.
    /// </summary>
    public static CultureInfo ToSortable(this CultureInfo culture)
    {
        var c = (CultureInfo)culture.Clone();
        c.DateTimeFormat.LongDatePattern = "yyyy'-'MM'-'dd";
        c.DateTimeFormat.LongTimePattern = "HH':'mm':'ss";
        c.DateTimeFormat.MonthDayPattern = "MM'-'dd";
        c.DateTimeFormat.YearMonthPattern = "yyyy'-'MM";
        c.DateTimeFormat.ShortDatePattern = "yyyy'-'MM'-'dd";
        c.DateTimeFormat.ShortTimePattern = "HH':'mm':'ss";
        return c;
    }

    /// <summary>
    /// <see cref="CultureInfo.InvariantCulture"/> with <see cref="ToSortable(CultureInfo)"/>.
    /// </summary>
    /// <remarks>
    /// It is insane that the invariant one uses MM/dd/yyyy.
    /// </remarks>
    public static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture.ToSortable();
}
