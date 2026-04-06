namespace TomRR.ResultType.Tests;

public class SuccessTests
{
    [Fact]
    public void Of_CreatesSuccessWithValue()
    {
        var success = Success<int>.Of(42);

        Assert.True(success.IsSuccessful);
        Assert.True(success.HasValue);
        Assert.Equal(42, success.Value);
    }

    [Fact]
    public void ImplicitConversion_CreatesSuccessWithValue()
    {
        Success<string> success = "hello";

        Assert.True(success.IsSuccessful);
        Assert.True(success.HasValue);
        Assert.Equal("hello", success.Value);
    }

    [Fact]
    public void IsSuccessful_WithNullValue_ReturnsFalse()
    {
        var success = Success<string?>.Of(null!);

        Assert.False(success.IsSuccessful);
        Assert.False(success.HasValue);
    }

    [Fact]
    public void ImplementsISuccessResult()
    {
        var success = Success<int>.Of(1);

        Assert.IsAssignableFrom<ISuccessResult>(success);
    }

    [Fact]
    public void EqualityByValue()
    {
        var a = Success<int>.Of(42);
        var b = Success<int>.Of(42);

        Assert.Equal(a, b);
    }

    [Fact]
    public void Inequality_DifferentValues()
    {
        var a = Success<int>.Of(1);
        var b = Success<int>.Of(2);

        Assert.NotEqual(a, b);
    }
}
