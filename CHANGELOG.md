# 📝 ResultType — Changelog

All notable changes to this project will be documented in this file.

This project follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)
and adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [0.1.0] — 2026-04-06
### ⚠️ Breaking Changes
- Standardized package identity to `TomRR.ResultType`:
    - `PackageId`: `TomRR.Core.ResultType` → `TomRR.ResultType`
    - `AssemblyName`: `ResultType` → `TomRR.ResultType`
    - `RootNamespace`: `ResultType` → `TomRR.ResultType`
- Updated all source and test namespaces:
    - `ResultType` → `TomRR.ResultType`
    - `ResultType.Tests` → `TomRR.ResultType.Tests`
- Changed `ErrorType` default/zero value semantics by introducing `ErrorType.None = 0`
  (`Failure` is no longer the default enum value).
- Changed `Error` property mutability:
    - `Description` and `ErrorType` are now get-only (no `init` setters).

### Changed
- Standardized `PackageReleaseNotes` to use `CHANGELOG.md` instead of inline notes
- Simplified solution platform configurations to:
    - `Debug|Any CPU`
    - `Release|Any CPU`
- `Error` now implements explicit value equality members (`IEquatable<Error>`, `==`, `!=`, `GetHashCode`).
- `Success<TValue>` now implements `ISuccessResult`.
- README release notes now point to `CHANGELOG.md` instead of embedding version history inline.
- Updated Apache license footer year format to `2025-present`.

### Added
- Functional composition APIs:
    - `Result<TValue, TError>.Map(...)`, `Bind(...)`, `MapError(...)`
    - `Result<TValue, TError, TStatusOnly>.Map(...)`, `Bind(...)`, `MapError(...)`
- Added `.NET 10` support (`net8.0;net9.0;net10.0`).
- Added a dedicated test project (`ResultType.Tests`) with coverage for:
    - `Result<TValue, TError>`
    - `Result<TValue, TError, TStatusOnly>`
    - `Error`, `Error<TValue>`, `Success<TValue>`, and unit/status-only result types
- Added CI/CD with GitHub Actions:
    - restore, format verification, build, and test on push/PR
    - NuGet package publish on version tags (`v*`)
- Added repository-wide defaults/configuration files:
    - `.editorconfig`
    - `Directory.Build.props`
- Enabled source debugging support:
    - `EmbedUntrackedSources`
    - `Microsoft.SourceLink.GitHub`
- Enabled XML documentation generation:
    - `<GenerateDocumentationFile>true</GenerateDocumentationFile>`
- Added `CHANGELOG.md` to NuGet package

---

## [0.0.6] — 2025-10-19
### Added
- Generic `Success<TValue>` and `Error<TValue>` with factory methods `SuccessOf()` and `ErrorOf()`.
- Marker interfaces: `ISuccessResult` and `IErrorResult`.
- New unit-only results: `Skipped`, `Timeout`, `Cancelled`, `Retried`, `None`, and `Empty`.

### Changed
- Updated documentation and README with new examples and structure.

---

## [0.0.5]
### Added
- `MemberNotNullWhen` attributes for `HasValue` and `HasError` to improve compiler flow analysis.
- `HasStatusOnly` property to indicate status-only results.
- Added `.NET 8` support.

---

## [0.0.4]
### Changed
- Updated README file — added a new section for implicit and explicit conversions.

---

## [0.0.3]
### Added
- `AssemblyName` entry in `.csproj` for clearer NuGet identity.

---

## [0.0.2]
### Added
- New `Result<TValue, TError, TStatusOnly>` type for success/failure/status-only scenarios.
- Support for `IStatusOnlyResult` constraint and status-only result handling.
- `Match` methods for improved pattern matching and error handling.
- Dedicated folder structure for built-in status-only results (`NoContent`, `Created`, etc.).

### Changed
- General architectural refinements.
- Improved internal organization and documentation clarity.

---

## [0.0.1]
### Added
- Initial version introducing a robust, type-safe, Rust-like `Result<TValue, TError>` model for explicit error handling.
- Support for `Success` and `Failure` result states.
- Nullability annotations via `MemberNotNullWhen`.
- Implicit conversions for seamless creation of success/error results.
- Internal state management with private constructors to ensure consistent instantiation.

### Notes
- Designed to promote clear, exception-free control flow in .NET applications.
