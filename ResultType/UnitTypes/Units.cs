namespace ResultType.UnitTypes;

/// <summary>
/// Represents a status-only result indicating that no content is returned (HTTP 204).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("NoContent")]
public readonly struct NoContent : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating the resource has not been modified (HTTP 304).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("NotModified")]
public readonly struct NotModified : IStatusOnlyResult;

/// <summary>
/// Represents a status-only result indicating a generic success (HTTP 200 OK).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("Success")]
public readonly struct Success;

/// <summary>
/// Represents a status-only result indicating that a new resource was created (HTTP 201).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("Created")]
public readonly struct Created;

/// <summary>
/// Represents a status-only result indicating that a request was accepted for asynchronous processing (HTTP 202).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("Accepted")]
public readonly struct Accepted;

/// <summary>
/// Represents a status-only result indicating that a resource was successfully deleted (custom status).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("Deleted")]
public readonly struct Deleted;

/// <summary>
/// Represents a status-only result indicating that a resource was successfully updated (custom status).
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay("Updated")]
public readonly struct Updated;