namespace ResultType.Core;

/// <summary>
/// Defines the types of errors that can be represented by an <see cref="Error" />.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A general failure that does not fit other categories.
    /// </summary>
    Failure,

    /// <summary>
    /// An unexpected error, such as an unhandled exception.
    /// </summary>
    Unexpected,

    /// <summary>
    /// A validation error caused by invalid input or business rules.
    /// </summary>
    Validation,

    /// <summary>
    /// A conflict occurred, typically related to state or resource versioning.
    /// </summary>
    Conflict,

    /// <summary>
    /// The requested resource was not found.
    /// </summary>
    NotFound,
}