namespace ResultType.Core;

/// <summary>
/// Defines the types of errors that can be represented by an <see cref="Error" />.
/// </summary>
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
}