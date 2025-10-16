namespace ResultType;

/// <summary>
/// Represents the state of a result object.
/// </summary>
public enum ResultState
{
    /// <summary>
    /// The operation was successful and returned a value.
    /// </summary>
    Success,

    /// <summary>
    /// The operation failed and returned an error.
    /// </summary>
    Failure,

    /// <summary>
    /// The operation completed with a status-only result (e.g., NoContent, NotModified).
    /// </summary>
    StatusOnly
}