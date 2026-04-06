namespace TomRR.ResultType.Tests;

public class ResultTValueTErrorTests
{
    [Fact]
    public void Success_Factory_CreatesSuccessfulResult()
    {
        var result = Result<int, Error>.Success(42);

        Assert.True(result.IsSuccessful);
        Assert.True(result.HasValue);
        Assert.False(result.HasFailed);
        Assert.False(result.HasError);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failed_Factory_CreatesFailedResult()
    {
        var error = Error.Validation("bad input");
        var result = Result<int, Error>.Failed(error);

        Assert.False(result.IsSuccessful);
        Assert.False(result.HasValue);
        Assert.True(result.HasFailed);
        Assert.True(result.HasError);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccess()
    {
        Result<string, Error> result = "hello";

        Assert.True(result.IsSuccessful);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromError_CreatesFailure()
    {
        var error = Error.NotFound("missing");
        Result<string, Error> result = error;

        Assert.True(result.HasFailed);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Match_OnSuccess_InvokesOnSuccess()
    {
        var result = Result<int, Error>.Success(10);

        var output = result.Match(
            onSuccess: v => $"ok:{v}",
            onFailure: e => $"err:{e.Description}");

        Assert.Equal("ok:10", output);
    }

    [Fact]
    public void Match_OnFailure_InvokesOnFailure()
    {
        var result = Result<int, Error>.Failed(Error.Conflict("clash"));

        var output = result.Match(
            onSuccess: v => $"ok:{v}",
            onFailure: e => $"err:{e.Description}");

        Assert.Equal("err:clash", output);
    }

    [Fact]
    public void Map_OnSuccess_TransformsValue()
    {
        var result = Result<int, Error>.Success(5);

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.IsSuccessful);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public void Map_OnFailure_PropagatesError()
    {
        var error = Error.Failure("oops");
        var result = Result<int, Error>.Failed(error);

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.HasFailed);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Bind_OnSuccess_ChainsResult()
    {
        var result = Result<int, Error>.Success(5);

        var bound = result.Bind(v =>
            v > 0
                ? Result<string, Error>.Success($"positive:{v}")
                : Result<string, Error>.Failed(Error.Validation("not positive")));

        Assert.True(bound.IsSuccessful);
        Assert.Equal("positive:5", bound.Value);
    }

    [Fact]
    public void Bind_OnSuccess_CanReturnFailure()
    {
        var result = Result<int, Error>.Success(-1);

        var bound = result.Bind(v =>
            v > 0
                ? Result<string, Error>.Success($"positive:{v}")
                : Result<string, Error>.Failed(Error.Validation("not positive")));

        Assert.True(bound.HasFailed);
        Assert.Equal("not positive", bound.Error.Description);
    }

    [Fact]
    public void Bind_OnFailure_PropagatesError()
    {
        var error = Error.Unexpected("boom");
        var result = Result<int, Error>.Failed(error);

        var bound = result.Bind(v => Result<string, Error>.Success($"val:{v}"));

        Assert.True(bound.HasFailed);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void MapError_OnFailure_TransformsError()
    {
        var result = Result<int, Error>.Failed(Error.Validation("bad"));

        var mapped = result.MapError(e => Error.Conflict($"wrapped: {e.Description}"));

        Assert.True(mapped.HasFailed);
        Assert.Equal(ErrorType.Conflict, mapped.Error.ErrorType);
        Assert.Equal("wrapped: bad", mapped.Error.Description);
    }

    [Fact]
    public void MapError_OnSuccess_PropagatesValue()
    {
        var result = Result<int, Error>.Success(42);

        var mapped = result.MapError(e => Error.Conflict($"wrapped: {e.Description}"));

        Assert.True(mapped.IsSuccessful);
        Assert.Equal(42, mapped.Value);
    }

    [Fact]
    public void HasValue_WithSuccessNullValue_ReturnsFalse()
    {
        // A success with null reference: IsSuccessful is true but HasValue is false
        var result = Result<string?, Error>.Success(null!);

        Assert.True(result.IsSuccessful);
        Assert.False(result.HasValue);
    }
}
