namespace StringInterpolation;

public static partial class Format
{
    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {Do(builder => builder.Append($"{...}")) } ..."
    /// </example>
    public static DoBuilder Do(DoAction action) => new(action);

    /// <summary>
    /// <see cref="Do(DoAction)"/>
    /// </summary>
    public delegate bool DoAction(SpanStringBuilder builder);

    /// <summary>
    /// <see cref="Do(DoAction)"/>
    /// </summary>
    public readonly struct DoBuilder : IBuilderFormattable
    {
        private readonly DoAction _action;
        public DoBuilder(DoAction action) => _action = action;
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format) => _action(builder);
    }

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {Do(state, (state, builder) => builder.Append($"{...}")) } ..."
    /// </example>
    public static DoBuilder<TState> Do<TState>(TState state, DoAction<TState> action) => new(state, action);

    /// <summary>
    /// <see cref="Do{TState}(TState, DoAction{TState})"/>
    /// </summary>
    public delegate bool DoAction<TState>(TState state, SpanStringBuilder builder);

    /// <summary>
    /// <see cref="Do{TState}(TState, DoAction{TState})"/>
    /// </summary>
    public readonly struct DoBuilder<TState> : IBuilderFormattable
    {
        private readonly DoAction<TState> _action;
        private readonly TState _state;
        public DoBuilder(TState state, DoAction<TState> action) => (_action, _state) = (action, state);
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format) => _action(_state, builder);
    }

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {Do((builder, format) => builder.Append($"{...}")) } ..."
    /// </example>
    public static FormatDoBuilder Do(FormatDoAction action) => new(action);

    /// <summary>
    /// <see cref="Do(FormatDoAction)"/>
    /// </summary>
    public delegate bool FormatDoAction(SpanStringBuilder builder, ReadOnlySpan<char> format);

    /// <summary>
    /// <see cref="Do(FormatDoAction)"/>
    /// </summary>
    public readonly struct FormatDoBuilder : IBuilderFormattable
    {
        private readonly FormatDoAction _action;
        public FormatDoBuilder(FormatDoAction action) => _action = action;
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format) => _action(builder, format);
    }

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {Do(state, (state, builder, format) => builder.Append($"{...}")) } ..."
    /// </example>
    public static FormatDoBuilder<TState> Do<TState>(TState state, FormatDoAction<TState> action) => new(state, action);

    /// <summary>
    /// <see cref="Do{TState}(TState, FormatDoAction{TState})"/>
    /// </summary>
    public delegate bool FormatDoAction<TState>(TState state, SpanStringBuilder builder, ReadOnlySpan<char> format);

    /// <summary>
    /// <see cref="Do{TState}(TState, FormatDoAction{TState})"/>
    /// </summary>
    public readonly struct FormatDoBuilder<TState> : IBuilderFormattable
    {
        private readonly FormatDoAction<TState> _action;
        private readonly TState _state;
        public FormatDoBuilder(TState state, FormatDoAction<TState> action) => (_action, _state) = (action, state);
        public bool Format(scoped SpanStringBuilder builder, ReadOnlySpan<char> format) => _action(_state, builder, format);
    }
}
