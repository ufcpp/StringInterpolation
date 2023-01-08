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
}
