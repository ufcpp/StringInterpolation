using System.Globalization;
using System.Text;

namespace StringInterpolationTest;

public class CultureSpedificStringBuilderTest
{
    [Fact]
    public void Culture()
    {
        Assert.Equal(
            "1,2",
            new StringBuilder()
                .With(CultureInfo.GetCultureInfo("fr-fr"))
                .Append(1.2)
                .ToString());

        Assert.Equal(
            "1,2 1,00 € 02/01/2000",
            new StringBuilder()
                .With(CultureInfo.GetCultureInfo("fr-fr"))
                .Append($"{1.2} {1:C} {new DateOnly(2000, 1, 2)}")
                .ToString());

        Assert.Equal(
            "1.2",
            new StringBuilder()
                .With(CultureInfo.GetCultureInfo("ja-jp"))
                .Append(1.2)
                .ToString());

        Assert.Equal(
            "1.2 ￥1 2000/01/02",
            new StringBuilder()
                .With(CultureInfo.GetCultureInfo("ja-jp"))
                .Append($"{1.2} {1:C} {new DateOnly(2000, 1, 2)}")
                .ToString());

        Assert.Equal(
            "1.2",
            new StringBuilder()
                .Invariant()
                .Append(1.2)
                .ToString());

        Assert.Equal(
            "1.2 ¤1.00 01/02/2000",
            new StringBuilder()
                .Invariant()
                .Append($"{1.2} {1:C} {new DateOnly(2000, 1, 2)}")
                .ToString());
    }

    [Fact]
    public void SortableInvariant()
    {
        Assert.Equal(
            "1.2",
            new StringBuilder()
                .SortableInvariant()
                .Append(1.2)
                .ToString());

        Assert.Equal(
            "1.2 ¤1.00 2000-01-02",
            new StringBuilder()
                .SortableInvariant()
                .Append($"{1.2} {1:C} {new DateOnly(2000, 1, 2)}")
                .ToString());
    }
}
