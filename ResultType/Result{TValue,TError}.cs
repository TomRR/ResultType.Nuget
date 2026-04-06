namespace TomRR.ResultType;

/// <summary>
/// Represents the result of an operation that can either succeed with a value of type <typeparamref name="TValue"/>
/// or fail with an error of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value produced on success.</typeparam>
/// <typeparam name="TError">The type of the error returned on failure.</typeparam>
public sealed partial record Result<TValue, TError> where TError : IErrorResult
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
    public static Result<TValue, TError> Success(TValue value) => value;

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

    /// <summary>
    /// Transforms the success value using the specified mapping function.
    /// If the result is a failure, the error is propagated unchanged.
    /// </summary>
    /// <typeparam name="TNew">The type of the new success value.</typeparam>
    /// <param name="mapper">A function to apply to the success value.</param>
    /// <returns>A new result with the mapped success value, or the original error.</returns>
    [Pure]
    public Result<TNew, TError> Map<TNew>(Func<TValue, TNew> mapper)
    {
        return _state switch
        {
            ResultState.Success => mapper(Value!),
            ResultState.Failure => Error!,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Chains a result-producing function onto the success value.
    /// If the result is a failure, the error is propagated unchanged.
    /// </summary>
    /// <typeparam name="TNew">The type of the new success value.</typeparam>
    /// <param name="binder">A function that takes the success value and returns a new result.</param>
    /// <returns>The result of the binder function, or the original error.</returns>
    [Pure]
    public Result<TNew, TError> Bind<TNew>(Func<TValue, Result<TNew, TError>> binder)
    {
        return _state switch
        {
            ResultState.Success => binder(Value!),
            ResultState.Failure => Error!,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Transforms the error value using the specified mapping function.
    /// If the result is a success, the value is propagated unchanged.
    /// </summary>
    /// <typeparam name="TNewError">The type of the new error.</typeparam>
    /// <param name="mapper">A function to apply to the error value.</param>
    /// <returns>A new result with the mapped error, or the original success value.</returns>
    [Pure]
    public Result<TValue, TNewError> MapError<TNewError>(Func<TError, TNewError> mapper)
        where TNewError : IErrorResult
    {
        return _state switch
        {
            ResultState.Success => Value!,
            ResultState.Failure => mapper(Error!),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
