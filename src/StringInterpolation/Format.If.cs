namespace StringInterpolation;

public static partial class Format
{
    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {
    ///     If(condition,
    ///         builder => builder.Append($"then {...}")
    ///     ) } ..."
    /// </example>
    public static DoBuilder If(bool predicate, DoAction thenAction)
        => predicate ? new(thenAction) : new(_ => true);

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {
    ///     If(condition,
    ///         builder => builder.Append($"then {...}"),
    ///         builder => builder.Append($"else {...}")
    ///     ) } ..."
    /// </example>
    public static DoBuilder If(bool predicate, DoAction thenAction, DoAction elseAction)
        => predicate ? new(thenAction) : new(elseAction);

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {
    ///     If(condition, state,
    ///         (state, builder) => builder.Append($"then {...}")
    ///     ) } ..."
    /// </example>
    public static DoBuilder<TState> If<TState>(bool predicate, TState state, DoAction<TState> thenAction)
        => predicate ? new(state, thenAction) : new(default!, (_, _) => true);

    /// <summary>
    /// </summary>
    /// <example>
    /// $"... {
    ///     If(condition, state,
    ///         (state, builder) => builder.Append($"then {...}"),
    ///         (state, builder) => builder.Append($"else {...}")
    ///     ) } ..."
    /// </example>
    public static DoBuilder<TState> If<TState>(bool predicate, TState state, DoAction<TState> thenAction, DoAction<TState> elseAction)
        => predicate ? new(state, thenAction) : new(state, elseAction);
}
