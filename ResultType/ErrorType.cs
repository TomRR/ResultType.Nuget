namespace TomRR.ResultType;

/// <summary>
/// Specifies the classification categories of errors represented by an <see cref="Error"/> instance.
/// This enumeration is used to indicate the general nature or origin of a failure within a result.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Represents an unset or uninitialized error type.
    /// This is the default value for <see cref="ErrorType"/>.
    /// </summary>
    None = 0,

    /// <summary>
    /// A general-purpose failure that does not fit any specific category.
    /// Typically used as a default when no more precise classification applies.
    /// </summary>
    Failure,

    /// <summary>
    /// An unexpected or unhandled error, such as an unanticipated exception or runtime fault.
    /// </summary>
    Unexpected,

    /// <summary>
    /// An error that occurs due to invalid input, rule violations, or failed validation logic.
    /// </summary>
    Validation,

    /// <summary>
    /// A conflict between the requested operation and the current state of the system or resource.
    /// Commonly used for concurrency or versioning issues.
    /// </summary>
    Conflict,

    /// <summary>
    /// Indicates that the requested resource or entity could not be found.
    /// </summary>
    NotFound,
}
