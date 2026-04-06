namespace TomRR.ResultType;

/// <summary>
/// Represents the result of an operation that has no success payload,
/// but can fail with an error value of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="TError">The type of the error returned on failure.</typeparam>
public sealed partial record Result<TError>
{
    private readonly ResultState _state;

    /// <summary>
    /// Gets the error when the result is in a failed state; otherwise, <c>null</c>.
    /// </summary>
    public readonly TError? Error;

    /// <summary>
    /// Indicates whether the result is successful.
    /// </summary>
    [Pure]
    public bool IsSuccessful => _state is ResultState.Success;

    /// <summary>
    /// Compatibility alias for <see cref="IsSuccessful"/>.
    /// </summary>
    [Pure]
    public bool IsSuccess => IsSuccessful;

    /// <summary>
    /// Indicates whether the result is in a failed state.
    /// </summary>
    [Pure]
    public bool HasFailed => _state is ResultState.Failure;

    /// <summary>
    /// Indicates whether this instance contains a non-null error value.
    /// </summary>
    [Pure]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool HasError => HasFailed && Error is not null;

    private Result()
    {
        _state = ResultState.Success;
        Error = default;
    }

    private Result(TError error)
    {
        _state = ResultState.Failure;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result with no payload.
    /// </summary>
    [Pure]
    public static Result<TError> Ok() => new();

    /// <summary>
    /// Creates a failed result containing the supplied error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    [Pure]
    public static Result<TError> Fail(TError error) => new(error);

    /// <summary>
    /// Implicitly creates a failed result from an error value.
    /// </summary>
    [Pure]
    public static implicit operator Result<TError>(TError error) => Fail(error);

    /// <summary>
    /// Pattern matches on the current result state.
    /// </summary>
    /// <typeparam name="TResult">The return type produced by each branch.</typeparam>
    /// <param name="onSuccess">Delegate invoked when the result is successful.</param>
    /// <param name="onFailure">Delegate invoked when the result is a failure.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onFailure)
    {
        return _state switch
        {
            ResultState.Success => onSuccess(),
            ResultState.Failure => onFailure(Error!),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Transforms the error value using the specified mapping function.
    /// If the result is successful, success is propagated unchanged.
    /// </summary>
    /// <typeparam name="TNewError">The type of the transformed error.</typeparam>
    /// <param name="mapper">A function to apply to the error value.</param>
    /// <returns>A new result with the mapped error, or the original success state.</returns>
    [Pure]
    public Result<TNewError> MapError<TNewError>(Func<TError, TNewError> mapper)
    {
        return _state switch
        {
            ResultState.Success => Result<TNewError>.Ok(),
            ResultState.Failure => mapper(Error!),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
