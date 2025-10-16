namespace ResultType;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue"></typeparam>
public sealed partial record Success<TValue>
{
    /// <summary>
    /// The value when the result is a success.
    /// Gets the value if the result is successful; otherwise, <c>null</c>.
    /// </summary>
    public readonly TValue? Value;
    
    private Success(TValue value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Implicitly creates a successful result from a value.
    /// </summary>
    [Pure]
    public static implicit operator Success<TValue>(TValue value) => new(value);
    
    /// <summary>
    /// Creates a general-purpose error with a specific description and type.
    /// </summary>
    public static Success<TValue> Of(TValue value) => new(value);

    /// <summary>
    /// Indicates whether the result is a success.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccessful => Value is not null;
    
    /// <summary>
    /// Indicates whether a non-null success value is available.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue => Value is not null;
}