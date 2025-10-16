namespace ResultType;

/// <summary>
/// Represents a strongly typed error result that encapsulates an error-related value or message.
/// Useful when returning contextual error information, such as validation details, exception messages, or error payloads.
/// </summary>
/// <typeparam name="TValue">
/// The type of the associated error value, such as a message, error object, or structured response.
/// </typeparam>
[DebuggerDisplay("Error: {Value}")]
public sealed partial record Error<TValue> : IErrorResult
{
    /// <summary>
    /// Gets the underlying value associated with the error.
    /// Typically contains additional context or diagnostic information related to the failure.
    /// </summary>
    public readonly TValue? Value;

    private Error(TValue value)
    {
        Value = value;
    }

    /// <summary>
    /// Implicitly creates a new <see cref="Error{TValue}"/> instance from the specified value.
    /// Enables seamless assignment of error values without explicit construction.
    /// </summary>
    /// <param name="value">The error-related value to wrap.</param>
    [Pure]
    public static implicit operator Error<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates a new <see cref="Error{TValue}"/> instance from the specified value.
    /// Use this when you prefer explicit construction instead of implicit conversion.
    /// </summary>
    /// <param name="value">The error-related value to wrap.</param>
    /// <returns>A new <see cref="Error{TValue}"/> instance.</returns>
    public static Error<TValue> Of(TValue value) => new(value);

    /// <summary>
    /// Indicates whether this instance represents a failed result.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsFailed => Value is not null;

    /// <summary>
    /// Indicates whether this instance contains an error value.
    /// This property is functionally equivalent to <see cref="IsFailed"/>.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasError => Value is not null;
}
