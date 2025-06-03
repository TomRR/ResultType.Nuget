using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace ResultType.Results;

public readonly struct Result<TValue, TError>
{
    private readonly ResultState _state;

    public readonly TValue? Value;
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
    
    public Result(TValue value, TError? error = default)
    {
        Value = value;
        Error = error;
        _state = ResultState.Success;
    }
    
    [Pure]
    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    [Pure]
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccessful => _state is ResultState.Success;

    [Pure]
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool HasFailed => _state is ResultState.Failure;
}