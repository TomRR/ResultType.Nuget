namespace ResultType;

/// <summary>
/// Represents a structured error with description and classification.
/// </summary>
[DebuggerDisplay("Error: {ErrorType} - {Description}")]
public readonly partial struct Error  : IErrorResult
{
  /// <summary>
  /// A human-readable description of the error.
  /// </summary>
    public string Description { get; init; }
  /// <summary>
  /// The category of the error.
  /// </summary>
    public ErrorType ErrorType { get; init; }

    private Error(string description, ErrorType errorType = default)
    {
        Description = description;
        ErrorType = errorType;
    }

    /// <summary>
    /// Creates a general-purpose error with a specific description and type.
    /// </summary>
    public static Error Create(string description, ErrorType errorType = default) => new(description, errorType);

    /// <summary>
    /// Creates a <see cref="Failure"/> error.
    /// </summary>
    public static Error Failure(string description = "A 'Failure' has occurred.") => new(description, ErrorType.Failure);

    /// <summary>
    /// Creates an <see cref="Unexpected"/> error.
    /// </summary>
    public static Error Unexpected(string description = "An 'Unexpected' error has occurred.") => new(description, ErrorType.Unexpected);

    /// <summary>
    /// Creates a <see cref="Validation"/> error.
    /// </summary>
    public static Error Validation(string description = "A 'Validation' error has occurred.") => new(description, ErrorType.Validation);

    /// <summary>
    /// Creates a <see cref="Conflict"/> error.
    /// </summary>
    public static Error Conflict(string description = "A 'Conflict' error has occurred.") => new(description, ErrorType.Conflict);

    /// <summary>
    /// Creates a <see cref="NotFound"/> error.
    /// </summary>
    public static Error NotFound(string description = "A 'Not Found' error has occurred.") => new(description, ErrorType.NotFound);
}