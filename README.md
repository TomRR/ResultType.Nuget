# ResultType

A lightweight, expressive, and extensible Result abstraction for .NET.

`ResultType` provides Rust-like result handling for C# with explicit success/failure/status-only flows, so you can model operation outcomes without using exceptions for control flow.

## Quick Links

- Sample verification guide: `ResultType.Sample/README.md`
- Run the sample: `dotnet run --project ResultType.Sample/ResultType.Sample.csproj -c Release`

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Basic Usage](#basic-usage)
  - [Result<TValue, TError>](#resulttvalue-terror)
  - [Result<TError>](#resultterror)
  - [Result<TValue, TError, TStatusOnly>](#resulttvalue-terror-tstatusonly)
- [Implicit and Explicit Creation](#implicit-and-explicit-creation)
- [Functional Composition](#functional-composition)
- [Generic Success and Error Wrappers](#generic-success-and-error-wrappers)
- [Naming and Type Clarification](#naming-and-type-clarification)
- [Built-in Status-Only Types](#built-in-status-only-types)
- [Custom Status-Only Type](#custom-status-only-type)
- [ASP.NET Core Integration Example](#aspnet-core-integration-example)
- [ResultType.Sample](#resulttypesample)
- [License](#license)
- [Changelog](#changelog)

## Features

- `Result<TValue, TError>` for success/failure outcomes
- `Result<TError>` for success/failure outcomes without a success payload
- `Result<TValue, TError, TStatusOnly>` for success/failure/status-only outcomes
- Implicit conversion from value/error/status-only markers
- Explicit factories for clarity: `Ok`, `Fail`, `Success`, `Failed`, `Status`
- Functional composition helpers: `Match`, `Map`, `Bind`, `MapError`
- Built-in status-only structs like `NoContent`, `Accepted`, `NotModified`, etc.
- Generic wrappers: `Success<TValue>`, `Error<TValue>`, `Result.SuccessOf(...)`, `Result.ErrorOf(...)`
- Easy to extend with your own `IStatusOnlyResult` structs

## Installation

```bash
dotnet add package TomRR.ResultType
```

Or in your project file:

```xml
<PackageReference Include="TomRR.ResultType" Version="X.X.X" />
```

## Basic Usage

### Result<TValue, TError>

`TError` must implement `IErrorResult`.

```csharp
using TomRR.ResultType;

Result<int, Error> Divide(int a, int b)
{
    if (b == 0)
    {
        return Error.Validation("Division by zero");
    }

    return a / b;
}

var result = Divide(10, 2);

var message = result.Match(
    onSuccess: value => $"Result: {value}",
    onFailure: error => $"Error: {error.Description}");
```

### Result<TError>

Use this form when success does not need to return a value.

```csharp
using TomRR.ResultType;

Result<Error> Save(bool canSave)
{
    if (!canSave)
    {
        return Result<Error>.Fail(Error.Validation("Cannot save"));
    }

    return Result<Error>.Ok();
}

var status = Save(false).Match(
    onSuccess: () => "Saved",
    onFailure: error => $"Error: {error.Description}");
```

### Result<TValue, TError, TStatusOnly>

Use this form when you need a third branch for status-only outcomes.

```csharp
using TomRR.ResultType;
using TomRR.ResultType.UnitTypes;

Result<string, Error, NoContent> TryFindItem(bool exists)
{
    if (exists)
    {
        return "Found item";
    }

    return Result.NoContent;
}

var response = TryFindItem(false).Match(
    onSuccess: value => $"Found: {value}",
    onFailure: error => $"Error: {error.Description}",
    onStatusOnly: _ => "No item found.");
```

## Implicit and Explicit Creation

You can create results either implicitly or via factory methods.

| Input | Produces |
| --- | --- |
| `TError` (single-arg) | `Result<TError>` failure |
| `TValue` | `Result<TValue, TError>` or `Result<TValue, TError, TStatusOnly>` success |
| `TError` | `Result<TValue, TError>` or `Result<TValue, TError, TStatusOnly>` failure |
| `TStatusOnly` | `Result<TValue, TError, TStatusOnly>` status-only |

Implicit example:

```csharp
Result<int, Error> Divide(int a, int b)
{
    if (b == 0)
    {
        return Error.Validation("Invalid divisor");
    }

    return a / b;
}
```

Explicit example:

```csharp
Result<int, Error, NoContent> Compute(bool shouldSkip, bool hasFailed)
{
    if (shouldSkip)
    {
        return Result<int, Error, NoContent>.Status(Result.NoContent);
    }

    if (hasFailed)
    {
        return Result<int, Error, NoContent>.Failed(Error.Failure("Failure occurred"));
    }

    return Result<int, Error, NoContent>.Success(42);
}
```

## Functional Composition

Both result types support `Match`, `Map`, `Bind`, and `MapError`.

```csharp
using TomRR.ResultType;

Result<int, Error> Parse(string input) =>
    int.TryParse(input, out var value)
        ? value
        : Error.Validation("Not a number");

Result<int, Error> EnsurePositive(int value) =>
    value > 0
        ? value
        : Error.Validation("Must be > 0");

var result = Parse("10")
    .Map(v => v * 2)                            // Result<int, Error>
    .Bind(EnsurePositive)                       // Result<int, Error>
    .MapError(e => Error.Conflict(e.Description));
```

## Generic Success and Error Wrappers

`Success<TValue>` and `Error<TValue>` are useful when you want typed payload wrappers.

```csharp
using TomRR.ResultType;

var success = Result.SuccessOf("Created item successfully");
var error = Result.ErrorOf("Invalid input");

Console.WriteLine(success.Value); // Created item successfully
Console.WriteLine(error.Value);   // Invalid input
```

You can also create them directly:

```csharp
Success<int> s = 42;
Error<string> e = "Failed";
```

## Naming and Type Clarification

- `Success<TValue>` wraps a success payload value.
- `Result.Success` is a status-only marker (`UnitTypes.Success`) with no payload.
- `Error` is a structured error type (`Description`, `ErrorType`).
- `Error<TValue>` wraps a typed error payload.
- `Result<TError>` models success/failure with no success payload.
- `Result<TValue, TError>` models success/failure with a success payload.

## Built-in Status-Only Types

All built-ins are zero-allocation `readonly struct` markers.

| Type | Typical meaning |
| --- | --- |
| `NoContent` | Success with no payload (HTTP 204 style) |
| `NotModified` | Resource unchanged (HTTP 304 style) |
| `Success` | Generic success marker |
| `Created` | Resource created (HTTP 201 style) |
| `Accepted` | Accepted for async processing (HTTP 202 style) |
| `Deleted` | Resource deleted |
| `Updated` | Resource updated |
| `Skipped` | Operation intentionally skipped |
| `Timeout` | Operation timed out |
| `Cancelled` | Operation cancelled |
| `Retried` | Operation retried |
| `None` | Neutral no-result outcome |
| `Empty` | Successful empty outcome |

Factory accessors are available via `Result`, for example:

```csharp
var status = Result.NoContent;
```

## Custom Status-Only Type

You can define your own status-only marker by implementing `IStatusOnlyResult`.

```csharp
using System.Diagnostics;
using System.Runtime.InteropServices;
using TomRR.ResultType;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Deferred))]
public readonly struct Deferred : IStatusOnlyResult;

Result<string, Error, Deferred> TryProcess(bool defer)
{
    if (defer)
    {
        return new Deferred();
    }

    return "Processed";
}
```

## ASP.NET Core Integration Example

`ToActionResult()` is not built into this package, but it is easy to add as an extension in your API project.

```csharp
using Microsoft.AspNetCore.Mvc;
using TomRR.ResultType;
using TomRR.ResultType.UnitTypes;

namespace MyApi;

public static class ResultHttpExtensions
{
    public static IActionResult ToActionResult<TValue, TError>(
        this Result<TValue, TError> result)
        where TError : IErrorResult
    {
        return result.Match<IActionResult>(
            onSuccess: value => new OkObjectResult(value),
            onFailure: error => new BadRequestObjectResult(error));
    }

    public static IActionResult ToActionResult<TValue, TError, TStatusOnly>(
        this Result<TValue, TError, TStatusOnly> result)
        where TError : IErrorResult
        where TStatusOnly : IStatusOnlyResult
    {
        return result.Match<IActionResult>(
            onSuccess: value => new OkObjectResult(value),
            onFailure: error => new BadRequestObjectResult(error),
            onStatusOnly: status => status switch
            {
                NoContent => new NoContentResult(),
                NotModified => new StatusCodeResult(304),
                _ => new StatusCodeResult(204)
            });
    }
}
```

## ResultType.Sample

This repository includes `ResultType.Sample`, a fail-fast verification console app.

It is designed to:

- exercise all main API combinations
- fail immediately if behavior changes unexpectedly
- provide a runnable walkthrough for users exploring the library

Run it with:

```bash
dotnet run --project ResultType.Sample/ResultType.Sample.csproj -c Release
```

For a detailed coverage map of what the sample verifies, see `ResultType.Sample/README.md`.

## License

Licensed under the Apache License 2.0.

## Changelog

See `[CHANGELOG.md](https://github.com/TomRR/ResultType.Nuget/blob/main/CHANGELOG.md)` for release history.
