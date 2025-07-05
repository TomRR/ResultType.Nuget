# Nuget: ResultType 

## 🚀 Usage

### Basic Example

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

With StatusOnly

```csharp
Result<string, string, NoContent> TryFindItem(bool found)
{
    if (found)
        return "Found item";

    return Result.NoContent;
}
```

---

## ✅ 4. **ASP.NET Core Integration: `ToIActionResult()`**

You can add this in an extensions file (`ResultExtensions.cs`) to bridge with MVC:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ResultType.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<TValue, TError>(this Result<TValue, TError> result)
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


## Add Your Own Unit Result

```csharp
public static partial class Result
{
    public static MyOwnResult MyOwnResult => new MyOwnResult();
}
```
```csharp
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct MyOwnResult : IStatusOnlyResult;
```