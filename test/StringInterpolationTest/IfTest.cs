using System.Text;
using static StringInterpolation.Format;

namespace StringInterpolationTest;

public class IfTest
{
    [Fact]
    public void NoArg()
    {
        var b = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            b.Append($"({If((i & 1) == 0,
                b => b.Append($"[{i}]")
                )})");
        }

        Assert.Equal("([0])()([2])()([4])()([6])()([8])()", b.ToString());
    }

    [Fact]
    public void NoArgElse()
    {
        var b = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            b.Append($"({If((i & 1) == 0,
                b => b.Append($"[{i}]"),
                b => b.Append($"<{i}>")
                )})");
        }

        Assert.Equal("([0])(<1>)([2])(<3>)([4])(<5>)([6])(<7>)([8])(<9>)", b.ToString());
    }

    [Fact]
    public void State()
    {
        var b = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            b.Append($"({If((i & 1) == 0, i,
                static (i, b) => b.Append($"[{i}]")
                )})");
        }

        Assert.Equal("([0])()([2])()([4])()([6])()([8])()", b.ToString());
    }

    [Fact]
    public void StateElse()
    {
        var b = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            b.Append($"({If((i & 1) == 0, i,
                static (i, b) => b.Append($"[{i}]"),
                static (i, b) => b.Append($"<{i}>")
                )})");
        }

        Assert.Equal("([0])(<1>)([2])(<3>)([4])(<5>)([6])(<7>)([8])(<9>)", b.ToString());
    }
}
