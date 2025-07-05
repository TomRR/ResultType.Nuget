namespace ResultType.UnitTypes;

/// <summary>
/// Provides factory accessors for common unit-style result types such as <see cref="NoContent" />, <see cref="Accepted" />, etc.
/// These types are e.g. used in APIs to represent status-only responses (e.g., HTTP 204 No Content, 202 Accepted).
/// </summary>
public static partial class Result
{
    /// <summary>
    /// Represents a result where no content is returned (e.g., HTTP 204 No Content).
    /// </summary>
    public static NoContent NoContent => new();

    /// <summary>
    /// Indicates that the resource has not been modified (e.g., HTTP 304 Not Modified).
    /// </summary>
    public static NotModified NotModified => new();

    /// <summary>
    /// Indicates a generic success with no payload (e.g., HTTP 200 OK).
    /// </summary>
    public static Success Success => new();

    /// <summary>
    /// Indicates that a resource was successfully created (e.g., HTTP 201 Created).
    /// </summary>
    public static Created Created => new();

    /// <summary>
    /// Indicates that a request has been accepted for processing (e.g., HTTP 202 Accepted).
    /// </summary>
    public static Accepted Accepted => new();

    /// <summary>
    /// Indicates that a resource was successfully deleted (custom semantic).
    /// </summary>
    public static Deleted Deleted => new();

    /// <summary>
    /// Indicates that a resource was successfully updated (custom semantic).
    /// </summary>
    public static Updated Updated => new();
}