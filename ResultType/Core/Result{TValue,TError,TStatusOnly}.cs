namespace ResultType.Core;

/// <summary>
/// Represents the result of an operation that may yield a value on success, an error on failure,
/// or a status-only outcome such as <c>NoContent</c> or <c>Accepted</c>.
/// </summary>
/// <typeparam name="TValue">The type of the value on success.</typeparam>
/// <typeparam name="TError">The type of the error on failure.</typeparam>
/// <typeparam name="TStatusOnly">The type representing a status-only result. Must implement <see cref="IStatusOnlyResult"/>.</typeparam>
public sealed partial record Result<TValue, TError, TStatusOnly> where TStatusOnly : IStatusOnlyResult
{
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

    /// <summary>
    /// The status-only value when the result is neither a success nor a failure.
    /// Gets the status-only marker; otherwise, <c>null</c>.
    /// </summary>
    public readonly TStatusOnly? StatusOnly;

    private readonly ResultState _state;

    private Result(TValue value)
    {
        Value = value;
        Error = default;
        StatusOnly = default;
        _state = ResultState.Success;
    }

    private Result(TError error)
    {
        Value = default;
        Error = error;
        StatusOnly = default;
        _state = ResultState.Failure;
    }
    
    private Result(TStatusOnly statusOnly)
    {
        Value = default;
        Error = default;
        StatusOnly = statusOnly;
        _state = ResultState.StatusOnly;
    }

    /// <summary>
    /// Implicitly creates a successful result from a value.
    /// </summary>
    [Pure]
    public static implicit operator Result<TValue, TError, TStatusOnly>(TValue value) => new(value);
    
    /// <summary>
    /// Implicitly creates a failed result from an error.
    /// </summary>
    [Pure]
    public static implicit operator Result<TValue, TError, TStatusOnly>(TError error) => new(error);
    
    /// <summary>
    /// Implicitly creates a status-only result.
    /// </summary>
    [Pure]
    public static implicit operator Result<TValue, TError, TStatusOnly>(TStatusOnly statusOnly) => new(statusOnly);
    
    /// <summary>
    /// Creates a success result with the given value.
    /// </summary>
    [Pure]
    public static Result<TValue, TError, TStatusOnly> Success(TValue value) => value;
    
    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    [Pure]
    public static Result<TValue, TError, TStatusOnly> Failed(TError error) => error;
    
    /// <summary>
    /// Creates a status-only result with the given value.
    /// </summary>
    [Pure]
    public static Result<TValue, TError, TStatusOnly> Status(TStatusOnly status) => status;

    /// <summary>
    /// Indicates whether the result is a success.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(false, nameof(StatusOnly))]
    public bool IsSuccessful => _state is ResultState.Success;

    /// <summary>
    /// Indicates whether a non-null success value is available.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(false, nameof(StatusOnly))]
    public bool HasValue => IsSuccessful && Value is not null;

    /// <summary>
    /// Indicates whether the result is a failure.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(StatusOnly))]
    public bool HasFailed => _state is ResultState.Failure;

    /// <summary>
    /// Indicates whether a non-null error is available.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(StatusOnly))]
    public bool HasError => HasFailed && Error is not null;

    /// <summary>
    /// Indicates whether the result is status-only.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(true, nameof(StatusOnly))]
    public bool IsStatusOnly => _state == ResultState.StatusOnly;
    
    /// <summary>
    /// Indicates whether a non-null status-only is available.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(true, nameof(StatusOnly))]
    public bool HasStatusOnly => _state == ResultState.StatusOnly;

    /// <summary>
    /// Pattern matches on all possible result cases: success, failure, or status-only.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match function.</typeparam>
    /// <param name="onSuccess">Invoked if the result is successful.</param>
    /// <param name="onFailure">Invoked if the result is a failure.</param>
    /// <param name="onStatusOnly">Invoked if the result is status-only.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<TError, TResult> onFailure,
        Func<TStatusOnly, TResult> onStatusOnly)
    {
        return _state switch
        {
            ResultState.Success => onSuccess(Value!),
            ResultState.Failure => onFailure(Error!),
            ResultState.StatusOnly => onStatusOnly(StatusOnly!),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
