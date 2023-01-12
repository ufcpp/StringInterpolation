using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace StringInterpolation;

public static partial class StringBuilderExtensions
{
    /// <summary>
    /// Associates <paramref name="builder"/> with <paramref name="provider"/>.
    /// </summary>
    /// <remarks>
    /// CultureInfo should be explicitly specified.
    ///
    /// see:
    /// https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1304
    /// https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md
    /// https://learn.microsoft.com/en-us/dotnet/standard/base-types/string-comparison-net-5-plus
    /// </remarks>
    public static CultureSpedificStringBuilder With(this StringBuilder builder, IFormatProvider provider) => new(builder, provider);

    /// <summary>
    /// <see cref="With(StringBuilder, IFormatProvider)"/> with <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static CultureSpedificStringBuilder Invariant(this StringBuilder builder) => new(builder, CultureInfo.InvariantCulture);

    /// <summary>
    /// <see cref="With(StringBuilder, IFormatProvider)"/> with <see cref="SortableDateTime.InvariantCulture"/>.
    /// </summary>
    public static CultureSpedificStringBuilder SortableInvariant(this StringBuilder builder) => new(builder, SortableDateTime.InvariantCulture);
}

public interface IBuilderProviderPair
{
    StringBuilder Builder { get; }
    IFormatProvider Provider { get; }
}

/// <summary>
/// Always specifies explicit <see cref="Provider"/> on appending values to <see cref="StringBuilder"/>.
/// </summary>
public class CultureSpedificStringBuilder : IBuilderProviderPair
{
    public StringBuilder Builder { get; }
    public IFormatProvider Provider { get; }

    public CultureSpedificStringBuilder(StringBuilder builder, IFormatProvider provider)
    {
        Builder = builder;
        Provider = provider;
    }

    public override string ToString() => Builder.ToString();

    public CultureSpedificStringBuilder Append(string value)
    {
        Builder.Append(value);
        return this;
    }

    public CultureSpedificStringBuilder Append(ReadOnlySpan<char> value)
    {
        Builder.Append(value);
        return this;
    }

    public CultureSpedificStringBuilder Append<T>(T value, IFormatProvider? provier = null)
        where T : ISpanFormattable
    {
        Builder.Append(provier ?? Provider, $"{value}");
        return this;
    }

    public CultureSpedificStringBuilder Append(
        [InterpolatedStringHandlerArgument("")]
        ref InterpolatedStringHandler handler
        ) => this;

    [InterpolatedStringHandler]
    public ref struct InterpolatedStringHandler
    {
        private StringBuilder.AppendInterpolatedStringHandler _inner;

        public InterpolatedStringHandler(int literalLength, int formattedCount, IBuilderProviderPair destination)
            => _inner = new(literalLength, formattedCount, destination.Builder, destination.Provider);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendLiteral(string value) => _inner.AppendLiteral(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted<T>(T value) => _inner.AppendFormatted(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted<T>(T value, string? format) => _inner.AppendFormatted(value, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted<T>(T value, int alignment) => _inner.AppendFormatted(value, alignment);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted<T>(T value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted(scoped ReadOnlySpan<char> value) => _inner.AppendFormatted(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted(scoped ReadOnlySpan<char> value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted(string? value) => _inner.AppendFormatted(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted(string? value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted(object? value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
    }
}
