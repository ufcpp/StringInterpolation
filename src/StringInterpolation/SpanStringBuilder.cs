using System.Runtime.CompilerServices;

namespace StringInterpolation;

/// <summary>
/// Pair of <see cref="Span{char}"/> and ref int
/// to write
/// <code><![CDATA[
/// bool TryWrite(SpanStringBuilder destination)
/// {
///     if (!destination.Append(a) return false;
///     if (!destination.Append(b) return false;
///     // ...
///     return true;
/// }
/// ]]></code>
/// instead of 
/// <code><![CDATA[
/// bool TryWrite(Span<char> destination, out int charsWritten)
/// {
///     if (destination.TryWrite(a, out var w) charsWritten += w; else return;
///     if (destination.TryWrite(b, out w) charsWritten += w; else return;
///     // ...
///     return true;
/// }
/// ]]></code>
/// </summary>
/// <remarks>
/// This internally uses <see cref="MemoryExtensions.TryWriteInterpolatedStringHandler"/>.
/// </remarks>
public ref struct SpanStringBuilder
{
    private Span<char> _destination;
    private readonly ref int _pos;
    private readonly IFormatProvider? _provider;

    public SpanStringBuilder(Span<char> destination, ref int charsWritten) : this(destination, ref charsWritten, null) { }

    public SpanStringBuilder(Span<char> destination, ref int charsWritten, IFormatProvider? provider)
    {
        _destination = destination;
        _pos = ref charsWritten;
        _provider = provider;
    }

    private Span<char> Destination => _destination[_pos..];

    public bool Success => _destination != default;

    private bool Fail()
    {
        _destination = default;
        return false;
    }

    private bool Ok(int charsWritten)
    {
        _pos += charsWritten;
        return true;
    }

    public bool Append(string value)
    {
        if (!Success) return false;
        return !value.TryCopyTo(Destination) ? Fail() : Ok(value.Length);
    }

    public bool Append<T>(T value, ReadOnlySpan<char> format = default, IFormatProvider? provier = null)
        where T : ISpanFormattable
    {
        if (!Success) return false;
        return !value.TryFormat(Destination, out var w, format, provier ?? _provider) ? Fail() : Ok(w);
    }

    public bool Append([InterpolatedStringHandlerArgument("")] scoped InterpolatedStringHandler handler)
    {
        if (!Success) return false;
        return !handler.TryGetResult(out var w) ? Fail() : Ok(w);
    }

    public bool Append(IFormatProvider? provider, [InterpolatedStringHandlerArgument("", nameof(provider))] scoped InterpolatedStringHandler handler)
    {
        if (!Success) return false;
        return !handler.TryGetResult(out var w) ? Fail() : Ok(w);
    }

    [InterpolatedStringHandler]
    public ref struct InterpolatedStringHandler
    {
        private MemoryExtensions.TryWriteInterpolatedStringHandler _inner;

        public InterpolatedStringHandler(int literalLength, int formattedCount, SpanStringBuilder destination, out bool shouldAppend)
            => _inner = new(literalLength, formattedCount, destination.Destination, destination._provider, out shouldAppend);

        public InterpolatedStringHandler(int literalLength, int formattedCount, SpanStringBuilder destination, IFormatProvider? provider, out bool shouldAppend)
            => _inner = new(literalLength, formattedCount, destination.Destination, provider ?? destination._provider, out shouldAppend);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendLiteral(string value) => _inner.AppendLiteral(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted<T>(T value) => _inner.AppendFormatted(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted<T>(T value, string? format) => _inner.AppendFormatted(value, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted<T>(T value, int alignment) => _inner.AppendFormatted(value, alignment);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted<T>(T value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted(scoped ReadOnlySpan<char> value) => _inner.AppendFormatted(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted(scoped ReadOnlySpan<char> value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted(string? value) => _inner.AppendFormatted(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AppendFormatted(string? value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
        internal bool TryGetResult(out int charsWritten) => default(Span<char>).TryWrite(ref _inner, out charsWritten);
    }
}
