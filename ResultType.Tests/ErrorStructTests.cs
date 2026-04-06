namespace TomRR.ResultType.Tests;

public class ErrorStructTests
{
    [Fact]
    public void Failure_CreatesFailureError()
    {
        var error = Error.Failure();

        Assert.Equal(ErrorType.Failure, error.ErrorType);
        Assert.Equal("A 'Failure' has occurred.", error.Description);
    }

    [Fact]
    public void Failure_WithCustomDescription()
    {
        var error = Error.Failure("custom failure");

        Assert.Equal(ErrorType.Failure, error.ErrorType);
        Assert.Equal("custom failure", error.Description);
    }

    [Fact]
    public void Unexpected_CreatesUnexpectedError()
    {
        var error = Error.Unexpected();

        Assert.Equal(ErrorType.Unexpected, error.ErrorType);
        Assert.Equal("An 'Unexpected' error has occurred.", error.Description);
    }

    [Fact]
    public void Unexpected_WithCustomDescription()
    {
        var error = Error.Unexpected("something unexpected");

        Assert.Equal(ErrorType.Unexpected, error.ErrorType);
        Assert.Equal("something unexpected", error.Description);
    }

    [Fact]
    public void Validation_CreatesValidationError()
    {
        var error = Error.Validation();

        Assert.Equal(ErrorType.Validation, error.ErrorType);
        Assert.Equal("A 'Validation' error has occurred.", error.Description);
    }

    [Fact]
    public void Validation_WithCustomDescription()
    {
        var error = Error.Validation("invalid email");

        Assert.Equal(ErrorType.Validation, error.ErrorType);
        Assert.Equal("invalid email", error.Description);
    }

    [Fact]
    public void Conflict_CreatesConflictError()
    {
        var error = Error.Conflict();

        Assert.Equal(ErrorType.Conflict, error.ErrorType);
        Assert.Equal("A 'Conflict' error has occurred.", error.Description);
    }

    [Fact]
    public void Conflict_WithCustomDescription()
    {
        var error = Error.Conflict("version mismatch");

        Assert.Equal(ErrorType.Conflict, error.ErrorType);
        Assert.Equal("version mismatch", error.Description);
    }

    [Fact]
    public void NotFound_CreatesNotFoundError()
    {
        var error = Error.NotFound();

        Assert.Equal(ErrorType.NotFound, error.ErrorType);
        Assert.Equal("A 'Not Found' error has occurred.", error.Description);
    }

    [Fact]
    public void NotFound_WithCustomDescription()
    {
        var error = Error.NotFound("user not found");

        Assert.Equal(ErrorType.NotFound, error.ErrorType);
        Assert.Equal("user not found", error.Description);
    }

    [Fact]
    public void Create_WithExplicitType()
    {
        var error = Error.Create("custom", ErrorType.Conflict);

        Assert.Equal(ErrorType.Conflict, error.ErrorType);
        Assert.Equal("custom", error.Description);
    }

    [Fact]
    public void Create_DefaultErrorType_IsNone()
    {
        var error = Error.Create("test");

        Assert.Equal(ErrorType.None, error.ErrorType);
    }

    [Fact]
    public void ImplementsIErrorResult()
    {
        var error = Error.Failure();

        Assert.IsAssignableFrom<IErrorResult>(error);
    }

    [Fact]
    public void Equals_SameDescriptionAndType_ReturnsTrue()
    {
        var a = Error.Validation("bad");
        var b = Error.Validation("bad");

        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Equals_DifferentDescription_ReturnsFalse()
    {
        var a = Error.Validation("bad");
        var b = Error.Validation("worse");

        Assert.False(a.Equals(b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var a = Error.Create("same", ErrorType.Validation);
        var b = Error.Create("same", ErrorType.Conflict);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_BoxedObject_ReturnsTrue()
    {
        var a = Error.Failure("x");
        object b = Error.Failure("x");

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_BoxedNonError_ReturnsFalse()
    {
        var a = Error.Failure("x");

        Assert.False(a.Equals("not an error"));
    }

    [Fact]
    public void GetHashCode_SameErrors_SameHash()
    {
        var a = Error.NotFound("missing");
        var b = Error.NotFound("missing");

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentErrors_LikelyDifferentHash()
    {
        var a = Error.NotFound("missing");
        var b = Error.Conflict("clash");

        Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void DefaultErrorType_IsNone()
    {
        Assert.Equal(ErrorType.None, default(ErrorType));
    }
}
