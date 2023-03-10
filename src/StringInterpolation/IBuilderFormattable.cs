namespace StringInterpolation;

/// <summary>
/// Implements <see cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
/// by using <see cref="SpanStringBuilder"/>.
/// </summary>
public interface IBuilderFormattable : IDefaultSpanFormattable
{
    bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format);

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var builder = new SpanStringBuilder(destination, out charsWritten, provider);
        return Format(builder, format);
    }
}
