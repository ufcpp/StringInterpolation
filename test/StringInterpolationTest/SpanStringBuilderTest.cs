namespace StringInterpolationTest;

public class SpanStringBuilderTest
{
    [Fact]
    public void Append()
    {
        var destination = (stackalloc char[10]);
        var builder = new SpanStringBuilder(destination, out var charsWritten);

        Assert.True(builder.Append("a"));
        Assert.True(builder.Success);
        Assert.Equal(1, charsWritten);
        Assert.Equal("a", destination[..charsWritten].ToString());

        Assert.True(builder.Append(12));
        Assert.True(builder.Success);
        Assert.Equal(3, charsWritten);
        Assert.Equal("a12", destination[..charsWritten].ToString());

        Assert.True(builder.Append($"a{1}b"));
        Assert.True(builder.Success);
        Assert.Equal(6, charsWritten);
        Assert.Equal("a12a1b", destination[..charsWritten].ToString());

        Assert.True(builder.Append("abc"));
        Assert.True(builder.Success);
        Assert.Equal(9, charsWritten);
        Assert.Equal("a12a1babc", destination[..charsWritten].ToString());

        Assert.False(builder.Append("ab"));
        Assert.False(builder.Success);
    }
}
