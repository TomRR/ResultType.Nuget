namespace ResultType.UnitTypes;

/// <summary>
/// Represents a status-only result indicating that no content is returned.
/// Commonly corresponds to HTTP 204 (No Content).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(NoContent))]
public readonly struct NoContent : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that the requested resource has not been modified since the last retrieval.
/// Commonly corresponds to HTTP 304 (Not Modified).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(NotModified))]
public readonly struct NotModified : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating a generic successful operation.
/// Commonly corresponds to HTTP 200 (OK).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Success))]
public readonly struct Success : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that a new resource was successfully created.
/// Commonly corresponds to HTTP 201 (Created).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Created))]
public readonly struct Created : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that a request has been accepted for asynchronous processing.
/// Commonly corresponds to HTTP 202 (Accepted).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Accepted))]
public readonly struct Accepted : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that a resource was successfully deleted.
/// This is a custom status that can be used to signal successful deletion operations.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Deleted))]
public readonly struct Deleted : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that a resource was successfully updated.
/// This is a custom status useful for signaling successful update operations.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Updated))]
public readonly struct Updated : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that a process or operation was intentionally skipped.
/// This is a custom status that can be used to denote bypassed or deferred work.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Skipped))]
public readonly struct Skipped : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that an operation did not complete due to a timeout.
/// This is a custom status that can be used for time-sensitive operations or workflows.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Timeout))]
public readonly struct Timeout : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that an operation was cancelled before completion.
/// This is a custom status that can be used for cooperative cancellation scenarios.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Cancelled))]
public readonly struct Cancelled : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating that an operation was retried after a previous attempt.
/// This is a custom status that can be used to signal repeated execution or recovery attempts.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Retried))]
public readonly struct Retried : IStatusOnlyResult;

/// <summary>
/// Represents a neutral result indicating that an operation produced no meaningful outcome.
/// Often used as a functional placeholder for "no result" scenarios.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(None))]
public readonly struct None : IStatusOnlyResult;

/// <summary>
/// Represents an empty result indicating a successful operation with no associated data.
/// Functionally similar to <see cref="NoContent"/>, but more general-purpose.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Empty))]
public readonly struct Empty : IStatusOnlyResult;
