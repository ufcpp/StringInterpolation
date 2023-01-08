using System.Globalization;
using static StringInterpolation.Format;

namespace StringInterpolationTest;

public class ForEachTest
{
    [Fact]
    public void NoArg()
    {
        Assert.Equal($"""
            [
                1,
                2,
                3,
                4,
            ]
            """,
            $"""
            [
            {ForEach(new[] { 1, 2, 3, 4 }, static (v, b) => b.Append($"""
                {v},

            """))}]
            """);

        Assert.Equal($"""
            [
                1.1,
                1.2,
                1.3,
                1.4,
            ]
            """,
           string.Create(CultureInfo.InvariantCulture, $"""
            [
            {ForEach(new[] { 1.1, 1.2, 1.3, 1.4 }, static (v, b) => b.Append($"""
                {v},

            """))}]
            """));

        Assert.Equal($"""
            [
                1,1,
                1,2,
                1,3,
                1,4,
            ]
            """,
           string.Create(CultureInfo.GetCultureInfo("fr-fr"), $"""
            [
            {ForEach(new[] { 1.1, 1.2, 1.3, 1.4 }, static (v, b) => b.Append($"""
                {v},

            """))}]
            """));
    }

    [Fact]
    public void State()
    {
        Assert.Equal($"""
            [
                5,
                10,
                15,
                20,
            ]
            """,
            $"""
            [
            {ForEach(new[] { 1, 2, 3, 4 }, 5, static (v, s, b) => b.Append($"""
                {v * s},

            """))}]
            """);

        Assert.Equal($"""
            [
                1.25,
                2.5,
                3.75,
                5,
            ]
            """,
           string.Create(CultureInfo.InvariantCulture, $"""
            [
            {ForEach(new[] { 1, 2, 3, 4 }, 1.25, static (v, s, b) => b.Append($"""
                {v * s},

            """))}]
            """));

        Assert.Equal($"""
            [
                1,25,
                2,5,
                3,75,
                5,
            ]
            """,
           string.Create(CultureInfo.GetCultureInfo("fr-fr"), $"""
            [
            {ForEach(new[] { 1, 2, 3, 4 }, 1.25, static (v, s, b) => b.Append($"""
                {v * s},

            """))}]
            """));
    }

    [Fact]
    public void Format()
    {
        Assert.Equal($"""
            [
                5,
                10,
                15,
                20,
            ]
            """,
            $"""
            [
            {ForEach(new[] { 5, 10, 15, 20 }, static (v, b) => b.Append($"""
                {v},

            """)):X}]
            """);

        Assert.Equal($"""
            [
                5,
                A,
                F,
                14,
            ]
            """,
            $"""
            [
            {ForEach(new[] { 5, 10, 15, 20 }, static (v, b, f) => b.Append($"""
                {v.ToString(f.ToString(), null)},

            """)):X}]
            """);
    }

    [Fact]
    public void StateFormat()
    {
        Assert.Equal($"""
            [
                5,
                10,
                15,
                20,
            ]
            """,
            $"""
            [
            {ForEach(new[] { 1, 2, 3, 4 }, 5, static (v, s, b) => b.Append($"""
                {v * s},

            """)):X}]
            """);

        Assert.Equal($"""
            [
                5,
                A,
                F,
                14,
            ]
            """,
            $"""
            [
            {ForEach(new[] { 1, 2, 3, 4 }, 5, static (v, s, b, f) => b.Append($"""
                {(v * s).ToString(f.ToString(), null)},

            """)):X}]
            """);
    }
}
