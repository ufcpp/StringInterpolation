using System.Globalization;
using static StringInterpolation.Format;

namespace StringInterpolationTest;

public class DoTest
{
    [Fact]
    public void NoArg()
    {
        Assert.Equal("[1A]",
            $"""
            [{Do(static b =>
            {
                b.Append(1);
                b.Append(10, "X");
                b.Append("""

                    """);
                return b.Success;
            })}]
            """);

        Assert.Equal("[1.1]",
           string.Create(CultureInfo.InvariantCulture, $"""
            [{Do(static b => b.Append(1.1))}]
            """));

        Assert.Equal("[1,1]",
           string.Create(CultureInfo.GetCultureInfo("fr-fr"), $"""
            [{Do(static b => b.Append(1.1))}]
            """));
    }

    [Fact]
    public void State()
    {
        Assert.Equal("[5A]",
            $"""
            [{Do(5, static (s, b) =>
            {
                b.Append(s);
                b.Append(2 * s, "X");
                b.Append("""

                    """);
                return b.Success;
            })}]
            """);
    }

    [Fact]
    public void Format()
    {
        Assert.Equal("[110]",
            $"""
            [{Do(static (b, f) =>
            {
                b.Append(1, f);
                b.Append(10, f);
                b.Append("""

                    """);
                return b.Success;
            })}]
            """);
        Assert.Equal("[110]",
            $"""
            [{Do(static (b, f) =>
            {
                b.Append(1, f);
                b.Append(10, f);
                b.Append("""

                    """);
                return b.Success;
            }):D}]
            """);
        Assert.Equal("[1A]",
            $"""
            [{Do(static (b, f) =>
            {
                b.Append(1, f);
                b.Append(10, f);
                b.Append("""

                    """);
                return b.Success;
            }):X}]
            """);
    }

    [Fact]
    public void StateFormat()
    {
        Assert.Equal("[510]",
            $"""
            [{Do(5, static (s, b, f) =>
            {
                b.Append(s, f);
                b.Append(2 * s, f);
                b.Append("""

                    """);
                return b.Success;
            })}]
            """);
        Assert.Equal("[510]",
            $"""
            [{Do(5, static (s, b, f) =>
            {
                b.Append(s, f);
                b.Append(2 * s, f);
                b.Append("""

                    """);
                return b.Success;
            }):D}]
            """);
        Assert.Equal("[5A]",
            $"""
            [{Do(5, static (s, b, f) =>
            {
                b.Append(s, f);
                b.Append(2 * s, f);
                b.Append("""

                    """);
                return b.Success;
            }):X}]
            """);
    }
}
