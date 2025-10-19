namespace ResultType;

/// <summary>
/// Provides factory methods for creating common result and status-only types.
/// This includes both value-based results (such as <see cref="Success{TValue}"/> or <see cref="Error{TValue}"/>)
/// and unit-style results (such as <see cref="NoContent"/>, <see cref="Accepted"/>, etc.).
/// <para>
/// These are particularly useful in APIs or domain operations to represent status-only responses,
/// e.g., HTTP 204 (No Content) or 202 (Accepted).
/// </para>
/// </summary>
public static partial class Result
{
    /// <summary>
    /// Creates a success result containing a value of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the success value.</typeparam>
    /// <param name="value">The value to include in the success result.</param>
    /// <returns>A <see cref="Success{TValue}"/> instance containing the specified value.</returns>
    [Pure]
    public static Success<TValue> SuccessOf<TValue>(TValue value) => Success<TValue>.Of(value);

    /// <summary>
    /// Creates an error result containing a value of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the error value.</typeparam>
    /// <param name="value">The value to include in the error result.</param>
    /// <returns>An <see cref="Error{TValue}"/> instance containing the specified value.</returns>
    [Pure]
    public static Error<TValue> ErrorOf<TValue>(TValue value) => Error<TValue>.Of(value);

    /// <summary>
    /// Represents a result where no content is returned (HTTP 204 No Content).
    /// </summary>
    public static NoContent NoContent => new();

    /// <summary>
    /// Represents a result indicating that the resource has not been modified (HTTP 304 Not Modified).
    /// </summary>
    public static NotModified NotModified => new();

    /// <summary>
    /// Represents a generic success with no payload (HTTP 200 OK).
    /// </summary>
    public static Success Success => new();

    /// <summary>
    /// Represents a result indicating that a resource was successfully created (HTTP 201 Created).
    /// </summary>
    public static Created Created => new();

    /// <summary>
    /// Represents a result indicating that a request has been accepted for asynchronous processing (HTTP 202 Accepted).
    /// </summary>
    public static Accepted Accepted => new();

    /// <summary>
    /// Represents a result indicating that a resource was successfully deleted (custom semantic).
    /// </summary>
    public static Deleted Deleted => new();

    /// <summary>
    /// Represents a result indicating that a resource was successfully updated (custom semantic).
    /// </summary>
    public static Updated Updated => new();

    /// <summary>
    /// Represents a result indicating that a process was skipped (custom semantic).
    /// </summary>
    public static Skipped Skipped => new();

    /// <summary>
    /// Represents a result indicating that a process timed out (custom semantic).
    /// </summary>
    public static Timeout Timeout => new();

    /// <summary>
    /// Represents a result indicating that a process was cancelled (custom semantic).
    /// </summary>
    public static Cancelled Cancelled => new();

    /// <summary>
    /// Represents a result indicating that a process was retried (custom semantic).
    /// </summary>
    public static Retried Retried => new();

    /// <summary>
    /// Represents a neutral result indicating that an operation completed
    /// without producing a success or error value.
    /// Useful for commands or operations that intentionally return no data.
    /// </summary>
    public static None None => new();

    /// <summary>
    /// Represents an empty result, typically used as a placeholder
    /// when an operation completes successfully but has no return value.
    /// </summary>
    public static Empty Empty => new();
}
