namespace ResultType;

/// <summary>
/// Marker interface for types that represent status-only (no payload) results (e.g., NoContent, NotModified).
/// </summary>
public interface IStatusOnlyResult : Unit;


/// <summary>
/// Marker interface representing a unit-type value (no payload, similar to 'void').
/// </summary>
// ReSharper disable once InconsistentNaming
public interface Unit;