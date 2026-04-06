namespace TomRR.ResultType.Tests;

public class ErrorGenericTests
{
    [Fact]
    public void Of_CreatesErrorWithValue()
    {
        var error = Error<string>.Of("something went wrong");

        Assert.True(error.IsFailed);
        Assert.True(error.HasError);
        Assert.Equal("something went wrong", error.Value);
    }

    [Fact]
    public void ImplicitConversion_CreatesErrorWithValue()
    {
        Error<string> error = "bad request";

        Assert.True(error.IsFailed);
        Assert.True(error.HasError);
        Assert.Equal("bad request", error.Value);
    }

    [Fact]
    public void IsFailed_WithNullValue_ReturnsFalse()
    {
        var error = Error<string?>.Of(null!);

        Assert.False(error.IsFailed);
        Assert.False(error.HasError);
    }

    [Fact]
    public void ImplementsIErrorResult()
    {
        var error = Error<string>.Of("err");

        Assert.IsAssignableFrom<IErrorResult>(error);
    }

    [Fact]
    public void EqualityByValue()
    {
        var a = Error<string>.Of("x");
        var b = Error<string>.Of("x");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Inequality_DifferentValues()
    {
        var a = Error<string>.Of("x");
        var b = Error<string>.Of("y");

        Assert.NotEqual(a, b);
    }
}
