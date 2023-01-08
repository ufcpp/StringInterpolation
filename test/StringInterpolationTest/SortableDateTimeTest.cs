using System.Globalization;

namespace StringInterpolationTest;

public class SortableDateTimeTest
{
    [Fact]
    public void ToSortable()
    {
        var cultures = new[]
        {
            (CultureInfo.InvariantCulture, "1.2", "¤1.00"),
            (CultureInfo.GetCultureInfo("ja-jp"), "1.2", "￥1"),
            (CultureInfo.GetCultureInfo("fr-fr"), "1,2", "1,00 €"),
        };

        var date = new DateOnly(2000, 1, 2);
        var time = new TimeOnly(3, 4, 5);
        var dt = date.ToDateTime(time, DateTimeKind.Unspecified);
        var dto = new DateTimeOffset(dt, TimeSpan.FromHours(9));

        var current = Thread.CurrentThread.CurrentCulture;

        foreach (var (c, expected1_2, expectedCurrency) in cultures)
        {
            Thread.CurrentThread.CurrentCulture = c.ToSortable();

            // Depends on culture.
            Assert.Equal(expected1_2, $"{1.2}");
            Assert.Equal(expectedCurrency, $"{1:C}");

            // Always uses sortable format for DateTime.
            Assert.Equal("2000-01-02", $"{dt:d}");
            Assert.Equal("2000-01-02", $"{dt:D}");
            Assert.Equal("2000-01-02 03:04:05", $"{dt:f}");
            Assert.Equal("2000-01-02 03:04:05", $"{dt:F}");
            Assert.Equal("2000-01", $"{dt:y}");
            Assert.Equal("01-02", $"{dt:m}");
            Assert.Equal("03:04:05", $"{dt:t}");

            Assert.Equal("2000-01-02", $"{date}");
            Assert.Equal("03:04:05", $"{time}");
            Assert.Equal("2000-01-02 03:04:05 +09:00", $"{dto}");
        }


        Thread.CurrentThread.CurrentCulture = current;
    }
}
