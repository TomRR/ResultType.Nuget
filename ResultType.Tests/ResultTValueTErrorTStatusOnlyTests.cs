namespace TomRR.ResultType.Tests;

public class ResultTValueTErrorTStatusOnlyTests
{
    [Fact]
    public void Success_Factory_CreatesSuccessfulResult()
    {
        var result = Result<int, Error, NoContent>.Success(42);

        Assert.True(result.IsSuccessful);
        Assert.True(result.HasValue);
        Assert.False(result.HasFailed);
        Assert.False(result.HasError);
        Assert.False(result.IsStatusOnly);
        Assert.False(result.HasStatusOnly);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failed_Factory_CreatesFailedResult()
    {
        var error = Error.Validation("bad input");
        var result = Result<int, Error, NoContent>.Failed(error);

        Assert.False(result.IsSuccessful);
        Assert.False(result.HasValue);
        Assert.True(result.HasFailed);
        Assert.True(result.HasError);
        Assert.False(result.IsStatusOnly);
        Assert.False(result.HasStatusOnly);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Status_Factory_CreatesStatusOnlyResult()
    {
        var result = Result<int, Error, NoContent>.Status(new NoContent());

        Assert.False(result.IsSuccessful);
        Assert.False(result.HasValue);
        Assert.False(result.HasFailed);
        Assert.False(result.HasError);
        Assert.True(result.IsStatusOnly);
        Assert.True(result.HasStatusOnly);
    }

    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccess()
    {
        Result<string, Error, Accepted> result = "hello";

        Assert.True(result.IsSuccessful);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromError_CreatesFailure()
    {
        var error = Error.NotFound("missing");
        Result<string, Error, Accepted> result = error;

        Assert.True(result.HasFailed);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromStatusOnly_CreatesStatusOnly()
    {
        Result<string, Error, Accepted> result = new Accepted();

        Assert.True(result.IsStatusOnly);
    }

    [Fact]
    public void Match_OnSuccess_InvokesOnSuccess()
    {
        var result = Result<int, Error, NoContent>.Success(10);

        var output = result.Match(
            onSuccess: v => $"ok:{v}",
            onFailure: e => $"err:{e.Description}",
            onStatusOnly: _ => "status");

        Assert.Equal("ok:10", output);
    }

    [Fact]
    public void Match_OnFailure_InvokesOnFailure()
    {
        var result = Result<int, Error, NoContent>.Failed(Error.Conflict("clash"));

        var output = result.Match(
            onSuccess: v => $"ok:{v}",
            onFailure: e => $"err:{e.Description}",
            onStatusOnly: _ => "status");

        Assert.Equal("err:clash", output);
    }

    [Fact]
    public void Match_OnStatusOnly_InvokesOnStatusOnly()
    {
        var result = Result<int, Error, NoContent>.Status(new NoContent());

        var output = result.Match(
            onSuccess: v => $"ok:{v}",
            onFailure: e => $"err:{e.Description}",
            onStatusOnly: _ => "status");

        Assert.Equal("status", output);
    }

    [Fact]
    public void Map_OnSuccess_TransformsValue()
    {
        var result = Result<int, Error, NoContent>.Success(5);

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.IsSuccessful);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public void Map_OnFailure_PropagatesError()
    {
        var error = Error.Failure("oops");
        var result = Result<int, Error, NoContent>.Failed(error);

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.HasFailed);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Map_OnStatusOnly_PropagatesStatus()
    {
        var result = Result<int, Error, NoContent>.Status(new NoContent());

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.IsStatusOnly);
    }

    [Fact]
    public void Bind_OnSuccess_ChainsResult()
    {
        var result = Result<int, Error, NoContent>.Success(5);

        var bound = result.Bind(v =>
            Result<string, Error, NoContent>.Success($"val:{v}"));

        Assert.True(bound.IsSuccessful);
        Assert.Equal("val:5", bound.Value);
    }

    [Fact]
    public void Bind_OnFailure_PropagatesError()
    {
        var error = Error.Unexpected("boom");
        var result = Result<int, Error, NoContent>.Failed(error);

        var bound = result.Bind(v =>
            Result<string, Error, NoContent>.Success($"val:{v}"));

        Assert.True(bound.HasFailed);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void Bind_OnStatusOnly_PropagatesStatus()
    {
        var result = Result<int, Error, NoContent>.Status(new NoContent());

        var bound = result.Bind(v =>
            Result<string, Error, NoContent>.Success($"val:{v}"));

        Assert.True(bound.IsStatusOnly);
    }

    [Fact]
    public void MapError_OnFailure_TransformsError()
    {
        var result = Result<int, Error, NoContent>.Failed(Error.Validation("bad"));

        var mapped = result.MapError(e => Error.Conflict($"wrapped: {e.Description}"));

        Assert.True(mapped.HasFailed);
        Assert.Equal(ErrorType.Conflict, mapped.Error.ErrorType);
    }

    [Fact]
    public void MapError_OnSuccess_PropagatesValue()
    {
        var result = Result<int, Error, NoContent>.Success(42);

        var mapped = result.MapError(e => Error.Conflict($"wrapped: {e.Description}"));

        Assert.True(mapped.IsSuccessful);
        Assert.Equal(42, mapped.Value);
    }

    [Fact]
    public void MapError_OnStatusOnly_PropagatesStatus()
    {
        var result = Result<int, Error, NoContent>.Status(new NoContent());

        var mapped = result.MapError(e => Error.Conflict($"wrapped: {e.Description}"));

        Assert.True(mapped.IsStatusOnly);
    }

    [Fact]
    public void HasValue_WithSuccessNullValue_ReturnsFalse()
    {
        var result = Result<string?, Error, NoContent>.Success(null!);

        Assert.True(result.IsSuccessful);
        Assert.False(result.HasValue);
    }
}
