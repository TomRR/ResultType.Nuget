# ResultType.Sample

`ResultType.Sample` is a fail-fast verification console app for `TomRR.ResultType`.

It serves two goals:

- quick regression detection when behavior changes
- runnable examples for developers exploring the API

## Run

```bash
dotnet run --project ResultType.Sample/ResultType.Sample.csproj -c Release
```

The app throws on the first failed check and exits with a non-zero code.

## Coverage Map

`Program.cs` is organized into focused verification blocks:

- `VerifyResultWithoutStatus`
  - Covers `Result<TValue, Error>`
  - Validates explicit and implicit creation, state flags, `Match`, `Map`, `Bind`, `MapError`, and nullable-success behavior

- `VerifyResultErrorOnly`
  - Covers `Result<TError>`
  - Validates `Ok`/`Fail`, implicit failure conversion, state flags, `Match`, `MapError`, and nullable-error behavior

- `VerifyResultWithStatus`
  - Covers `Result<TValue, Error, TStatusOnly>`
  - Validates success/failure/status-only branches, implicit conversions, `Match`, `Map`, `Bind`, `MapError`, and nullable-success behavior

- `VerifySuccessAndErrorWrappers`
  - Covers `Success<TValue>`, `Error<TValue>`, `Result.SuccessOf(...)`, `Result.ErrorOf(...)`, and `Error` factory methods

- `VerifyBuiltInStatusTypes`
  - Covers all built-in status-only markers exposed by `Result`
  - Confirms each type works in a status-only branch and keeps expected unit-struct size

- `VerifyCustomStatusType`
  - Demonstrates custom extension with `IStatusOnlyResult` (`Deferred`)
  - Confirms it integrates with `Result<TValue, TError, TStatusOnly>` and `Match`

## Adding New API Features

When adding new behavior in the library:

1. Extend the matching verification block in `ResultType.Sample/Program.cs`.
2. Keep checks deterministic and fail-fast.
3. Run the sample and unit tests locally before commit.

This keeps sample documentation and regression signals aligned.
