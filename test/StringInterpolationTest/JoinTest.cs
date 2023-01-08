using System.Globalization;

namespace StringInterpolationTest;

public class JoinTest
{
    [Fact]
    public void Join()
    {
        Assert.Equal(
            "a,b,c,d",
            $"{Format.Join(",", new[] { "a", "b", "c", "d" })}");

        Assert.Equal(
            "abc, def, ghi, jkl",
            $"{Format.Join(", ", new[] { "abc", "def", "ghi", "jkl" })}");

        Assert.Equal(
            string.Join(" / ", Enumerable.Repeat("123456789abcdefghijk", 100)),
            $"{Format.Join(" / ", Enumerable.Repeat("123456789abcdefghijk", 100))}");
    }

    [Fact]
    public void JoinT()
    {
        Assert.Equal(
            "1,12,123,1234",
            $"{Format.Join(",", new[] { 1, 12, 123, 1234 })}");

        Assert.Equal(
            "1000, 1000000, 1000000000, 1000000000000",
            $"{Format.Join(", ", new long[] { 1000, 1000000, 1000000000, 1000000000000 })}");

        Assert.Equal(
            string.Join(" / ", Enumerable.Repeat(123456789, 100)),
            $"{Format.Join(" / ", Enumerable.Repeat(123456789, 100))}");
    }

    [Fact]
    public void JoinWithCulture()
    {
        var jp = CultureInfo.GetCultureInfo("ja-jp");
        var fr = CultureInfo.GetCultureInfo("fr-fr");

        var nums = new[] { 1.2, 1.5, 1.7 };
        var dates = new[] { new DateOnly(2000, 1, 2), new DateOnly(2020, 12, 31) };

        Assert.Equal(
            "1.2/1.5/1.7",
            string.Create(jp, $"{Format.Join("/", nums)}"));

        Assert.Equal(
            "1,2/1,5/1,7",
            string.Create(fr, $"{Format.Join("/", nums)}"));

        Assert.Equal(
            "2000/01/02, 2020/12/31",
            string.Create(jp, $"{Format.Join(", ", dates)}"));

        Assert.Equal(
            "02/01/2000, 31/12/2020",
            string.Create(fr, $"{Format.Join(", ", dates)}"));
    }

    [Fact]
    public void JoinWithFormat()
    {
        var nums = new[] { 5, 10, 15, 20 };
        var dates = new[] { new DateOnly(2000, 1, 2), new DateOnly(2020, 12, 31) };

        Assert.Equal(
            "5/10/15/20",
            $"{Format.Join("/", nums):d}");

        Assert.Equal(
            "5/a/f/14",
            $"{Format.Join("/", nums):x}");

        Assert.Equal(
            "05/0A/0F/14",
            $"{Format.Join("/", nums):X2}");

        Assert.Equal(
            "01-2000, 12-2020",
            $"{Format.Join(", ", dates):MM-yyyy}");
    }
}
