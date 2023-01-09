namespace StringInterpolation;

public static partial class Format
{
    /// <summary>
    /// Joins <see cref="ISpanFormattable"/>s without temprary string allocation.
    /// </summary>
    /// <remarks>
    /// <see cref="Join(string, IEnumerable{string})"/>
    /// </remarks>
    public static JoinFormattable<T> Join<T>(string separator, IEnumerable<T> values)
        where T : ISpanFormattable
        => new(separator, values);

    /// <summary>
    /// <see cref="Join{T}(string, IEnumerable{T})"/>
    /// </summary>
    public readonly struct JoinFormattable<T> : IBuilderFormattable
        where T : ISpanFormattable
    {
        public readonly string Separator;
        public readonly IEnumerable<T> Values;
        public JoinFormattable(string separator, IEnumerable<T> values) => (Separator, Values) = (separator, values);

        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            bool part = false;
            foreach (var value in Values)
            {
                if (!part) part = true;
                else if (!builder.Append(Separator)) return false;

                if (!builder.Append(value, format)) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Concatenates <see cref="ISpanFormattable"/>s without temprary string allocation.
    /// </summary>
    public static JoinFormattable<T> Concat<T>(IEnumerable<T> values)
        where T : ISpanFormattable
        => new("", values);
}
