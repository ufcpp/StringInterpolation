using System.Globalization;

namespace StringInterpolationTest;

public class IBuilderFormattableTest
{
    private class Format : IBuilderFormattable
    {
        bool IBuilderFormattable.Format(SpanStringBuilder builder, ReadOnlySpan<char> format)
            => builder.Append($"format:{format}");
    }

    [Fact]
    public void WithFormat()
    {
        var formats = new[]
        {
            "x", "X4", "yyyy-MM-dd", "any string"
        };

        IFormattable a = new Format();

        foreach (var f in formats)
        {
            var actual = a.ToString(f, CultureInfo.InvariantCulture);
            Assert.Equal($"format:{f}", actual);
        }
    }

    private class Provider : IBuilderFormattable
    {
        public bool Format(SpanStringBuilder builder, ReadOnlySpan<char> format)
            => builder.Append($"{1.2}/{new DateOnly(2000, 1, 2)}");
    }

    [Fact]
    public void WithProvider()
    {
        var providers = new[]
        {
            (CultureInfo.InvariantCulture, "1.2/01/02/2000"),
            (CultureInfo.GetCultureInfo("ja-jp"), "1.2/2000/01/02"),
            (CultureInfo.GetCultureInfo("fr-fr"), "1,2/02/01/2000"),
        };

        IFormattable a = new Provider();

        foreach (var (p, expected) in providers)
        {
            var actual = a.ToString(null, p);
            Assert.Equal(expected, actual);
        }
    }

    private class LongerThan1000 : IBuilderFormattable
    {
        public bool Format(SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            int i = 0;
            for (; i < 100; i++)
            {
                if (!builder.Append($"0123456789abcdef")) return false;
            }
            return true;
        }
    }

    [Fact]
    public void WriteLongerThan1000Chars()
    {
        var expected = string.Join("", Enumerable.Range(0, 100).Select(_ => "0123456789abcdef"));

        IFormattable a = new LongerThan1000();
        var actual = a.ToString(null, null);

        Assert.Equal(expected, actual);
    }
}
