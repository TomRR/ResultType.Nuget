namespace TomRR.ResultType;

/// <summary>
/// Represents a strongly typed success result that encapsulates a value produced by a successful operation.
/// Useful for returning context or payloads associated with a successful outcome, such as created entities,
/// computation results, or confirmation messages.
/// This type wraps success payloads and is distinct from the status-only <see cref="UnitTypes.Success"/> marker
/// exposed via <see cref="Result.Success"/>.
/// </summary>
/// <typeparam name="TValue">
/// The type of the value associated with the success result.
/// </typeparam>
[DebuggerDisplay("Success: {Value}")]
public sealed partial record Success<TValue> : ISuccessResult
{
    /// <summary>
    /// Gets the underlying value associated with this success result.
    /// Typically contains the output or data returned by a successful operation.
    /// </summary>
    public readonly TValue? Value;

    private Success(TValue value)
    {
        Value = value;
    }

    /// <summary>
    /// Implicitly creates a new <see cref="Success{TValue}"/> instance from the specified value.
    /// Enables concise assignment of successful values without explicit construction.
    /// </summary>
    /// <param name="value">The success-related value to wrap.</param>
    [Pure]
    public static implicit operator Success<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates a new <see cref="Success{TValue}"/> instance from the specified value.
    /// Use this method when you prefer explicit construction rather than relying on the implicit operator.
    /// </summary>
    /// <param name="value">The success-related value to wrap.</param>
    /// <returns>A new <see cref="Success{TValue}"/> instance containing the provided value.</returns>
    public static Success<TValue> Of(TValue value) => new(value);

    /// <summary>
    /// Indicates whether this instance contains a non-null success payload.
    /// A <c>null</c> payload is considered "no successful value" for this wrapper.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccessful => Value is not null;

    /// <summary>
    /// Indicates whether a non-null success value is available.
    /// This property is equivalent to <see cref="IsSuccessful"/> and included for semantic clarity.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue => Value is not null;
}
