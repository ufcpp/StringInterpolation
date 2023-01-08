namespace StringInterpolation;

public static partial class Format
{
    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {ForEach(values, (value, builder) => builder.Append($"{value}")) } ..."
    /// </example>
    public static ForEachBuilder<TValue> ForEach<TValue>(IEnumerable<TValue> values, ForEachAction<TValue> action) => new(values, action);

    /// <summary>
    /// <see cref="ForEach{TValue, T}(IEnumerable{TValue}, T, ForEachAction{TValue, T})"/>
    /// </summary>
    public delegate bool ForEachAction<TValue>(TValue value, SpanStringBuilder builder);

    /// <summary>
    /// <see cref="ForEach{TValue}(IEnumerable{TValue}, ForEachAction{TValue})"/>
    /// </summary>
    public readonly struct ForEachBuilder<TValue> : IBuilderFormattable
    {
        private readonly IEnumerable<TValue> _values;
        private readonly ForEachAction<TValue> _action;
        public ForEachBuilder(IEnumerable<TValue> values, ForEachAction<TValue> action) => (_values, _action) = (values, action);
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            foreach (var value in _values)
                if (!_action(value, builder)) return false;
            return true;
        }
    }

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {ForEach(values, state, (value, state, builder) => builder.Append($"{value} {state}")) } ..."
    /// </example>
    public static ForEachBuilder<TValue, TState> ForEach<TValue, TState>(IEnumerable<TValue> values, TState state, ForEachAction<TValue, TState> action) => new(values, state, action);

    /// <summary>
    /// <see cref="ForEach{TValue, TState}(IEnumerable{TValue}, TState, ForEachAction{TValue, TState})"/>
    /// </summary>
    public delegate bool ForEachAction<TValue, TState>(TValue value, TState state, SpanStringBuilder builder);

    /// <summary>
    /// <see cref="ForEach{TValue, TState}(IEnumerable{TValue}, TState, ForEachAction{TValue, TState})"/>
    /// </summary>
    public readonly struct ForEachBuilder<TValue, TState> : IBuilderFormattable
    {
        private readonly IEnumerable<TValue> _values;
        private readonly ForEachAction<TValue, TState> _action;
        private readonly TState _state;
        public ForEachBuilder(IEnumerable<TValue> values, TState state, ForEachAction<TValue, TState> action) => (_values, _action, _state) = (values, action, state);
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            foreach (var value in _values)
                if (!_action(value, _state, builder)) return false;
            return true;
        }
    }

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {ForEach(values, (value, builder, format) => builder.Append($"{value}")) } ..."
    /// </example>
    public static FormatForEachBuilder<TValue> ForEach<TValue>(IEnumerable<TValue> values, FormatForEachAction<TValue> action) => new(values, action);

    /// <summary>
    /// <see cref="ForEach{TValue}(IEnumerable{TValue}, FormatForEachAction{TValue})"/>
    /// </summary>
    public delegate bool FormatForEachAction<TValue>(TValue value, SpanStringBuilder builder, ReadOnlySpan<char> format);

    /// <summary>
    /// <see cref="ForEach{TValue}(IEnumerable{TValue}, FormatForEachAction{TValue})"/>
    /// </summary>
    public readonly struct FormatForEachBuilder<TValue> : IBuilderFormattable
    {
        private readonly IEnumerable<TValue> _values;
        private readonly FormatForEachAction<TValue> _action;
        public FormatForEachBuilder(IEnumerable<TValue> values, FormatForEachAction<TValue> action) => (_values, _action) = (values, action);
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            foreach (var value in _values)
                if (!_action(value, builder, format)) return false;
            return true;
        }
    }

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {ForEach(values, state, (value, state, builder, format) => builder.Append($"{value} {state}")) } ..."
    /// </example>
    public static FormatForEachBuilder<TValue, TState> ForEach<TValue, TState>(IEnumerable<TValue> values, TState state, FormatForEachAction<TValue, TState> action) => new(values, state, action);

    /// <summary>
    /// <see cref="ForEach{TValue, TState}(IEnumerable{TValue}, TState, FormatForEachAction{TValue, TState})"/>
    /// </summary>
    public delegate bool FormatForEachAction<TValue, TState>(TValue value, TState state, SpanStringBuilder builder, ReadOnlySpan<char> format);

    /// <summary>
    /// <see cref="ForEach{TValue, TState}(IEnumerable{TValue}, TState, FormatForEachAction{TValue, TState})"/>
    /// </summary>
    public readonly struct FormatForEachBuilder<TValue, TState> : IBuilderFormattable
    {
        private readonly IEnumerable<TValue> _values;
        private readonly FormatForEachAction<TValue, TState> _action;
        private readonly TState _state;
        public FormatForEachBuilder(IEnumerable<TValue> values, TState state, FormatForEachAction<TValue, TState> action) => (_values, _action, _state) = (values, action, state);
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format)
        {
            foreach (var value in _values)
                if (!_action(value, _state, builder, format)) return false;
            return true;
        }
    }
}
