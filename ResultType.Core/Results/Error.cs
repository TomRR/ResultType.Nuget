namespace ResultType.Results;

public readonly partial struct Error
{
    public string Description { get; init; }
    public ErrorType ErrorType { get; init; }

    private Error(string description, ErrorType errorType = default)
    {
        Description = description;
        ErrorType = errorType;
    }

    public static Error Create(string description, ErrorType errorType = default)
    {
        return new Error(description, errorType);
    }
    
    /// <summary>
    /// Creates an <see cref="T:ErrorOr.Error" /> of type <see cref="F:ErrorOr.ErrorType.Failure" /> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Failure(string description = "A failure has occurred.")
    {
      return new Error(description, ErrorType.Failure);
    }

    /// <summary>
    /// Creates an <see cref="T:ErrorOr.Error" /> of type <see cref="F:ErrorOr.ErrorType.Unexpected" /> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Unexpected(string description = "An unexpected error has occurred.")
    {
      return new Error(description, ErrorType.Unexpected);
    }

    /// <summary>
    /// Creates an <see cref="T:ErrorOr.Error" /> of type <see cref="F:ErrorOr.ErrorType.Validation" /> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Validation(string description = "A validation error has occurred.")
    {
      return new Error(description, ErrorType.Validation);
    }

    /// <summary>
    /// Creates an <see cref="T:ErrorOr.Error" /> of type <see cref="F:ErrorOr.ErrorType.Conflict" /> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Conflict(string description = "A conflict error has occurred.")
    {
      return new Error(description, ErrorType.Conflict);
    }

    /// <summary>
    /// Creates an <see cref="T:ErrorOr.Error" /> of type <see cref="F:ErrorOr.ErrorType.NotFound" /> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    public static Error NotFound(string description = "A 'Not Found' error has occurred.")
    {
      return new Error(description, ErrorType.NotFound);
    }

    // /// <summary>
    // /// Creates an <see cref="T:ErrorOr.Error" /> with the given numeric <paramref name="type" />,
    // /// <paramref name="code" />, and <paramref name="description" />.
    // /// </summary>
    // /// <param name="type">An integer value which represents the type of error that occurred.</param>
    // /// <param name="code">The unique error code.</param>
    // /// <param name="description">The error description.</param>
    public static Error Custom(string description , ErrorType type)
    {
      return new Error(description, type);
    }
}