using System.Diagnostics;

namespace ResultType;

/// <summary>
/// Represents a standardized error with a human-readable description and an associated classification type.
/// This type is typically used to describe failures in a structured way (e.g., validation errors, conflicts, or unexpected conditions).
/// </summary>
[DebuggerDisplay("Error: {ErrorType} - {Description}")]
public readonly partial struct Error : IErrorResult
{
    /// <summary>
    /// Gets the human-readable description of the error.
    /// This value provides additional context about what went wrong.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Gets the category or classification of the error.
    /// This helps distinguish between different error domains, such as validation, conflict, or system failure.
    /// </summary>
    public ErrorType ErrorType { get; init; }

    private Error(string description, ErrorType errorType = default)
    {
        Description = description;
        ErrorType = errorType;
    }

    /// <summary>
    /// Creates a general-purpose error with a specific description and classification type.
    /// </summary>
    /// <param name="description">A human-readable description of the error.</param>
    /// <param name="errorType">An optional classification for the error. Defaults to <see cref="ErrorType.None"/>.</param>
    /// <returns>A new <see cref="Error"/> instance representing the specified error condition.</returns>
    public static Error Create(string description, ErrorType errorType = default) => new(description, errorType);

    /// <summary>
    /// Creates a <see cref="Failure"/> error representing a general operation failure.
    /// </summary>
    /// <param name="description">An optional description of the failure. Defaults to a generic message.</param>
    /// <returns>An <see cref="Error"/> categorized as a failure.</returns>
    public static Error Failure(string description = "A 'Failure' has occurred.") =>
        new(description, ErrorType.Failure);

    /// <summary>
    /// Creates an <see cref="Unexpected"/> error representing an unforeseen or unhandled condition.
    /// </summary>
    /// <param name="description">An optional description of the unexpected error. Defaults to a generic message.</param>
    /// <returns>An <see cref="Error"/> categorized as unexpected.</returns>
    public static Error Unexpected(string description = "An 'Unexpected' error has occurred.") =>
        new(description, ErrorType.Unexpected);

    /// <summary>
    /// Creates a <see cref="Validation"/> error representing input or data validation issues.
    /// </summary>
    /// <param name="description">An optional description of the validation error. Defaults to a generic message.</param>
    /// <returns>An <see cref="Error"/> categorized as a validation failure.</returns>
    public static Error Validation(string description = "A 'Validation' error has occurred.") =>
        new(description, ErrorType.Validation);

    /// <summary>
    /// Creates a <see cref="Conflict"/> error representing a conflict in the current operation or resource state.
    /// </summary>
    /// <param name="description">An optional description of the conflict. Defaults to a generic message.</param>
    /// <returns>An <see cref="Error"/> categorized as a conflict.</returns>
    public static Error Conflict(string description = "A 'Conflict' error has occurred.") =>
        new(description, ErrorType.Conflict);

    /// <summary>
    /// Creates a <see cref="NotFound"/> error representing a missing resource or entity.
    /// </summary>
    /// <param name="description">An optional description of the missing resource. Defaults to a generic message.</param>
    /// <returns>An <see cref="Error"/> categorized as not found.</returns>
    public static Error NotFound(string description = "A 'Not Found' error has occurred.") =>
        new(description, ErrorType.NotFound);
}
