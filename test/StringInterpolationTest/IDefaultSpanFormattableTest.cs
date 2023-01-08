using System.Globalization;

namespace StringInterpolationTest;

public class IDefaultSpanFormattableTest
{
    private class Format : IDefaultSpanFormattable
    {
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            if (!destination.TryWrite($"format:{format}", out charsWritten)) return false;
            return true;
        }
    }

    [Fact]
    public void FormatFormat()
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

    private class Provider : IDefaultSpanFormattable
    {
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            Assert.NotNull(provider);
            if (provider is not CultureInfo c) throw new InvalidOperationException();
            if (!destination.TryWrite($"provider:{c.DisplayName}", out charsWritten)) return false;
            return true;
        }
    }

    [Fact]
    public void FormatProvider()
    {
        var providers = new[]
        {
            CultureInfo.InvariantCulture,
            CultureInfo.GetCultureInfo("ja-jp"),
            CultureInfo.GetCultureInfo("fr-fr"),
        };

        IFormattable a = new Provider();

        foreach (var p in providers)
        {
            var actual = a.ToString(null, p);
            Assert.Equal($"provider:{p.DisplayName}", actual);
        }
    }

    private class LongerThan1000 : IDefaultSpanFormattable
    {
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            charsWritten = 0;

            {
                if (!destination[charsWritten..].TryWrite($"format:{format}/", out var w)) return false;
                charsWritten += w;
            }
            {
                if (!destination[charsWritten..].TryWrite($"provider:{provider?.GetType().Name}/", out var w)) return false;
                charsWritten += w;
            }

            for (int i = 0; i < 100; i++)
            {
                if (!destination[charsWritten..].TryWrite($"0123456789", out var w)) return false;
                charsWritten += w;
            }

            return true;
        }
    }

    [Fact]
    public void FormatLongerThan1000Chars()
    {
        var expected = $"format:abcd/provider:{CultureInfo.InvariantCulture.GetType().Name}/"
            + string.Join("", Enumerable.Range(0, 100).Select(_ => "0123456789"));

        IFormattable a = new LongerThan1000();
        var actual = a.ToString("abcd", CultureInfo.InvariantCulture);

        Assert.Equal(expected, actual);
    }
}
