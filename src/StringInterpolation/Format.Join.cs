namespace StringInterpolation;

public static partial class Format
{
    /// <summary>
    /// Joins strings without temprary string allocation.
    /// </summary>
    /// <remarks>
    /// Comparing to <see cref="string.Join(string?, IEnumerable{string?})"/>,
    /// this method is better in terms of memory allocation but not always better in performance.
    /// In general, this is slower than string.Join when the values is too long.
    /// If you want Join long sequence, you should use enough initialBuffer explicitly.
    /// e.g.
    ///
    /// <code><![CDATA[
    /// var buffer = ArrayPool<char>.Shared.Rent(enoughSize);
    /// var s = string.Create(null, buffer,
    ///     $"{Format.Join(", ", LongSequence())}");
    /// ArrayPool<char>.Shared.Return(buffer);
    /// return s;
    /// ]]></code>
    /// </remarks>
    public static JoinFormattable Join(string separator, IEnumerable<string> values) => new(separator, values);

    /// <summary>
    /// <see cref="Join(string, IEnumerable{string})"/>
    /// </summary>
    public readonly struct JoinFormattable : IBuilderFormattable
    {
        public readonly string Separator;
        public readonly IEnumerable<string> Values;
        public JoinFormattable(string separator, IEnumerable<string> values) => (Separator, Values) = (separator, values);

        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            bool part = false;
            foreach (var value in Values)
            {
                if (!part) part = true;
                else if (!builder.Append(Separator)) return false;

                if (!builder.Append(value)) return false;
            }
            return true;
        }
    }
}
