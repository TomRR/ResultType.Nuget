# 📦 ResultType

A lightweight, expressive, and extensible result abstraction for .NET — with support for structured results, errors, and unit-only responses (like `NoContent`, `Accepted`, etc).

---

## ✨ Features

- ✅ Simple `Result<TSuccess, TError>` model
- ✅ Optional support for `StatusOnly` results
- ✅ Clean pattern matching with `.Match(...)`
- ✅ ASP.NET Core integration via `ToActionResult()`
- ✅ Easily extensible with your own unit result types

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

### Result<TSuccess, TError, TStatusOnly>

Supports status-only outcomes like NoCoÍntent, Accepted, etc.
```csharp
Result<string, string, NoContent> TryFindItem(bool found)
{
    if (found)
        return "Found item";

    return Result.NoContent;
}

var response = TryFindItem(false).Match(
    success => $"Found: {success}",
    error => $"Error: {error}",
    status => "No item found."
);
```
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

## 🧩 Custom Unit (StatusOnly) Results

Define your own unit-only result:
```
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct MyOwnResult : IStatusOnlyResult;
```

Expose it via the factory:
```csharp
public static partial class Result
{
    public static MyOwnResult MyOwnResult => new MyOwnResult();
}
```

Use just like built-in NoContent, Created, etc.  

## 🧰 Built-in Unit Result Types
| Result Type   | Description                            | HTTP Status |
| ------------- | -------------------------------------- | ----------- |
| `NoContent`   | Operation successful, no content       | 204         |
| `Accepted`    | Request accepted for async processing  | 202         |
| `Created`     | New resource created                   | 201         |
| `Success`     | Generic success                        | 200         |
| `Deleted`     | Resource successfully deleted (custom) | Custom      |
| `Updated`     | Resource successfully updated (custom) | Custom      |
| `NotModified` | Resource not modified                  | 304         |

All are defined as *zero-alloc* `readonly struct`.

## 📦 Installation

Install via NuGet:
```
dotnet add package ResultType
```
Or via your .csproj:
```
<PackageReference Include="ResultType" Version="0.0.2" />
```

## 📄 License

Licensed under the Apache License 2.0.