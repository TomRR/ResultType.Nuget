# 📦 ResultType

---
A lightweight, expressive, and extensible result abstraction for .NET — with support for structured results, errors, generic success/error, and unit-only responses (like `NoContent`, `Accepted`, `None`, `Empty`, etc).

---
## 📑 Table of Contents
- [✨ Features](#-features)
- [🚀 Basic Usage](#-basic-usage)
    - [Result\<TSuccess, TError\>](#resulttsuccess-terror)
    - [Result\<TSuccess, TError, TStatusOnly\>](#resulttsuccess-terror-tstatusonly)
- [➕ Implicit & Explicit Conversions](#-implicit--explicit-conversions)
    - [✅ Implicit Example](#-implicit-example)
    - [📎 Explicit Example](#-explicit-example)
- [🧱 Generic Success and Error Results](#-generic-success-and-error-results)
- [🧩 Custom Unit (StatusOnly) Results](#-custom-unit-statusonly-results)
- [🧰 Built-in Unit Result Types](#-built-in-unit-result-types)
- [🌐 ASP.NET Core Integration](#-aspnet-core-integration)
- [✅ Controller Example](#-controller-example)
- [📦 Installation](#-installation)
- [📄 License](#-license)
- [📝 Release Notes / Changelog](#-release-notes--changelog)
---

## ✨ Features

- ✅ Simple `Result<TSuccess, TError>` model
- ✅ Optional support for `StatusOnly` results
- ✅ Generic `Success<TValue>` and `Error<TValue>` for carrying payloads
- ✅ Unit-style results: `NoContent`, `Accepted`, `Created`, `Skipped`, `Timeout`, `Cancelled`, `Retried`, `None`, `Empty`
- ✅ Clean pattern matching with `.Match(...)`
- ✅ ASP.NET Core integration via `ToActionResult()`
- ✅ Easily extensible with custom unit result types

---

## 🚀 Basic Usage

### `Result<TSuccess, TError>`

```csharp
Result<int, string> Divide(int a, int b)
{
    if (b == 0)
        return "Division by zero";

    return a / b;
}

var result = Divide(10, 0);

var message = result.Match(
    success => $"Result: {success}",
    error => $"Error: {error}"
);
```

Your Result types support clean and expressive ways to return values from functions — using either implicit conversion or explicit factory methods.

### Result<TSuccess, TError, TStatusOnly>

Supports status-only outcomes like NoContent, Accepted, Skipped, etc.

```csharp
Result<string, Error, NoContent> TryFindItem(bool? found)
{
    if (found)
    {    
        return "Found item";
    }
    else
    {    
        return Error.NotFound("Found not item");
    }
   

    return Result.NoContent;
}

var response = TryFindItem(false).Match(
    success => $"Found: {success}",
    error => $"Error: {error}",
    status => "No item found."
);
```

## ➕ Implicit & Explicit Conversions

You can return results using implicit conversions for clean and expressive code:
| Input Type    | Result Type                                 |
| ------------- | ------------------------------------------- |
| `TValue`      | `Result<TValue, TError>` or with StatusOnly |
| `TError`      | `Result<TValue, TError>` or with StatusOnly |
| `TStatusOnly` | `Result<TValue, TError, TStatusOnly>`       |

### ✅ Implicit Example
```csharp
Result<int, string> Divide(int a, int b)
{
    if (b == 0)
        return "Invalid"; // implicitly creates an error

    return a / b; // implicitly creates a success
}
```

```csharp
Result<string, string, NoContent> TryGet(bool exists)
{
    if (!exists)
        return Result.NoContent;

    return "value";
}

```

### 📎 Explicit Example

Prefer explicit factory methods if clarity is desired:
```csharp
Result<int, Error, NoContent> Compute()
{
    if (ShouldSkip())
        return Result<int, Error, NoContent>.Status(Result.NoContent);

    if (HasFailed())
        return Result<int, Error, NoContent>.Failed(Error.Failure("Failure occurred"));

    return Result<int, Error, NoContent>.Success(42);
}

```

Available factory methods:

    Result<TValue, TError, TStatusOnly>.Success(value)

    Result<TValue, TError, TStatusOnly>.Failed(error)

    Result<TValue, TError, TStatusOnly>.Status(statusOnly)

## 🧱 Generic Success and Error Results

Use strongly typed `Success<TValue>` and `Error<TValue>` when you need to wrap or carry structured payloads.
```csharp
using ResultType;

var success = Result.SuccessOf("Created item successfully");
var error = Result.ErrorOf("Invalid input");

Console.WriteLine(success.Value); // Created item successfully
Console.WriteLine(error.Value);   // Invalid input
```

### ✅ Success<TValue>

Represents a successful operation with a payload.
```csharp
var s1 = new Success<string>("Done");
var s2 = Success<string>.Of("Completed");
var s3 = Result.SuccessOf("Saved successfully");
```

### ❌ Error<TValue>

Represents a failure with an associated error value.
```csharp
var e1 = new Error<string>("File not found");
var e2 = Error<string>.Of("Invalid request");
var e3 = Result.ErrorOf("Timeout occurred");
```
Each supports implicit conversion for concise usage:
```csharp
Success<int> s = 42;       // implicit
Error<string> e = "Failed"; // implicit
```
Both types implement semantic convenience properties:
| Property                      | Description                                   |
| ----------------------------- | --------------------------------------------- |
| `HasValue`                    | Indicates if the underlying value is non-null |
| `IsSuccessful` (Success only) | Indicates a valid success payload             |

## 🧩 Extending with Custom Unit Results

You can define your own `IStatusOnlyResult` types to represent domain-specific outcomes.

```csharp
[StructLayout(LayoutKind.Sequential, Size = 1)]
[DebuggerDisplay(nameof(Deferred))]
public readonly struct CustomResult : IStatusOnlyResult;

public static partial class Result
{
    public static CustomResult CustomResult => new();
}

Use just like built-in NoContent, Created, etc.

## 🧰 Built-in Unit Result Types
| Result Type   | Description                                  | HTTP Status / Notes |
| ------------- | -------------------------------------------- | ------------------- |
| `NoContent`   | Operation successful, no content             | 204                 |
| `Accepted`    | Request accepted for async processing        | 202                 |
| `Created`     | New resource created                         | 201                 |
| `Success`     | Generic success                              | 200                 |
| `Deleted`     | Resource successfully deleted (custom)       | Custom              |
| `Updated`     | Resource successfully updated (custom)       | Custom              |
| `NotModified` | Resource not modified                        | 304                 |
| `Skipped`     | Process skipped (custom)                     | Custom              |
| `Timeout`     | Process timed out (custom)                   | Custom              |
| `Cancelled`   | Process cancelled (custom)                   | Custom              |
| `Retried`     | Process retried (custom)                     | Custom              |
| `None`        | Neutral, no-result outcome                   | Custom              |
| `Empty`       | Successful operation with no associated data | Custom              |

All are *zero-alloc* `readonly struct`.



## 🌐 ASP.NET Core Integration

Add this extension to convert Result into IActionResult:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ResultType.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<TValue, TError>(
        this Result<TValue, TError> result)
    {
        return result.Match<IActionResult>(
            value => new OkObjectResult(value),
            error => new BadRequestObjectResult(error)
        );
    }

    public static IActionResult ToActionResult<TValue, TError, TStatusOnly>(
        this Result<TValue, TError, TStatusOnly> result)
        where TStatusOnly : IStatusOnlyResult
    {
        return result.Match<IActionResult>(
            value => new OkObjectResult(value),
            error => new BadRequestObjectResult(error),
            statusOnly => statusOnly switch
            {
                NoContent => new NoContentResult(),
                NotModified => new StatusCodeResult(304),
                _ => new StatusCodeResult(204)
            }
        );
    }
}
```

## ✅ Controller Example

```csharp
[HttpGet]
public IActionResult GetData() =>
    TryFindItem(true).ToActionResult();
```

## 📦 Installation

Install via NuGet:
```
dotnet add package TomRR.Core.ResultType
```
Or via your .csproj:
```
<PackageReference Include="TomRR.Core.ResultType" Version="X.X.X" />
```

## 📄 License

Licensed under the Apache License 2.0.

## 📝 Release Notes / Changelog
### 0.0.6:
- Added generic Success<TValue> and Error<TValue> with factory methods SuccessOf() and ErrorOf()
- Introduced marker interfaces: ISuccessResult, IErrorResult
- Added new unit results: Skipped, Timeout, Cancelled, Retried, None, Empty
- Updated Documentation

### 0.0.5:
- Added `MemberNotNullWhen` attributes to `HasValue` and `HasError` (improve compiler flow analysis).
- Introduced the new `HasStatusOnly` property to explicitly indicate when a result represents a status-only state.
- Added .Net 8 support

### 0.0.4:
- Updated Readme file - added section for implicit and explicit conversions

### 0.0.3: 
- Added AssemblyName to the .csproj

### 0.0.2: Architectural Refinements and API Enhancements
- Introduced a second specialized Result types: `Result<TValue, TError, TStatusOnly>` for success/failure/status-only scenarios.
- Added support for status-only results with constraint on IStatusOnlyResult.
- Added match methods for better pattern matching and error handling.
- Consolidated status-only types (`NoContent`, `Created`, etc.) into a dedicated location for better organization.
- General code cleanup and documentation improvements.

### 0.0.1: Initial Testing Version: Rust-Like Result Type
- Introduced a robust and type-safe "Rust-Like" Result type for explicit error handling in C#/.NET projects.
- Result<TValue, TError>
- Supports result states: Success, Failure.
- Type safety and nullability analysis with comprehensive `MemberNotNullWhen` attributes.
- Internal state management with private constructors, encouraging controlled instantiation.
- Designed to promote more reliable and readable code by avoiding exceptions for control flow.
- Supports implicit conversions for easy result creation.