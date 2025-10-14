namespace ResultType.Core;

/// <summary>
/// Represents the result of an operation that can either succeed with a value of type <typeparamref name="TValue"/>
/// or fail with an error of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value produced on success.</typeparam>
/// <typeparam name="TError">The type of the error returned on failure.</typeparam>
public sealed partial record Result<TValue, TError>
{
    private readonly ResultState _state;

    /// <summary>
    /// The value when the result is a success.
    /// Gets the value if the result is successful; otherwise, <c>null</c>.
    /// </summary>
    public readonly TValue? Value;

    /// <summary>
    /// The error when the result is a failure.
    /// Gets the error if the result is a failure; otherwise, <c>null</c>.
    /// </summary>
    public readonly TError? Error;

    private Result(TValue value)
    {
        Value = value;
        Error = default;
        _state = ResultState.Success;
    }

    private Result(TError error)
    {
        Value = default;
        Error = error;
        _state = ResultState.Failure;
    }
    
    /// <summary>
    /// Implicitly creates a successful result from a value.
    /// </summary>
    [Pure]
    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    /// <summary>
    /// Implicitly creates a failed result from an error.
    /// </summary>
    [Pure]
    public static implicit operator Result<TValue, TError>(TError error) => new(error);
    
    /// <summary>
    /// Creates a success result with the given value.
    /// </summary>
    [Pure]
    public static Result<TValue, TError> Success(TValue value)=> value;
    
    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    [Pure]
    public static Result<TValue, TError> Failed(TError error) => error;

    /// <summary>
    /// Indicates whether the result is a success.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccessful => _state is ResultState.Success;
    
    /// <summary>
    /// Indicates whether a non-null success value is available.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool HasValue => IsSuccessful && Value is not null;

    /// <summary>
    /// Indicates whether the result is a failure.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool HasFailed => _state is ResultState.Failure;
    
    /// <summary>
    /// Indicates whether a non-null error is available.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool HasError => HasFailed && Error is not null;
    

    /// <summary>
    /// Pattern matches on the result, invoking the appropriate delegate.
    /// </summary>
    /// <typeparam name="TResult">The type returned by all pattern branches.</typeparam>
    /// <param name="onSuccess">Delegate to invoke when the result is successful.</param>
    /// <param name="onFailure">Delegate to invoke when the result is a failure.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<TError, TResult> onFailure)
    {
        return _state switch
        {
            ResultState.Success => onSuccess(Value!),
            ResultState.Failure => onFailure(Error!),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}