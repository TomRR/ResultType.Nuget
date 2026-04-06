namespace TomRR.ResultType.Tests;

public class ResultTErrorTests
{
    [Fact]
    public void Ok_CreatesSuccessfulResult()
    {
        var result = Result<Error>.Ok();

        Assert.True(result.IsSuccessful);
        Assert.True(result.IsSuccess);
        Assert.False(result.HasFailed);
        Assert.False(result.HasError);
    }

    [Fact]
    public void Fail_CreatesFailedResult()
    {
        var error = Error.Validation("bad input");
        var result = Result<Error>.Fail(error);

        Assert.False(result.IsSuccessful);
        Assert.False(result.IsSuccess);
        Assert.True(result.HasFailed);
        Assert.True(result.HasError);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromError_CreatesFailure()
    {
        Result<Error> result = Error.Conflict("version mismatch");

        Assert.True(result.HasFailed);
        Assert.True(result.HasError);
        Assert.Equal(ErrorType.Conflict, result.Error.ErrorType);
    }

    [Fact]
    public void Match_OnSuccess_InvokesOnSuccess()
    {
        var result = Result<Error>.Ok();

        var output = result.Match(
            onSuccess: () => "ok",
            onFailure: error => $"err:{error.Description}");

        Assert.Equal("ok", output);
    }

    [Fact]
    public void Match_OnFailure_InvokesOnFailure()
    {
        var result = Result<Error>.Fail(Error.NotFound("missing"));

        var output = result.Match(
            onSuccess: () => "ok",
            onFailure: error => $"err:{error.Description}");

        Assert.Equal("err:missing", output);
    }

    [Fact]
    public void MapError_OnFailure_TransformsError()
    {
        var result = Result<Error>.Fail(Error.Validation("bad"));

        var mapped = result.MapError(error => Error.Unexpected($"wrapped:{error.Description}"));

        Assert.True(mapped.HasFailed);
        Assert.True(mapped.HasError);
        Assert.Equal(ErrorType.Unexpected, mapped.Error.ErrorType);
        Assert.Equal("wrapped:bad", mapped.Error.Description);
    }

    [Fact]
    public void MapError_OnSuccess_PreservesSuccess()
    {
        var result = Result<Error>.Ok();

        var mapped = result.MapError(error => Error.Unexpected($"wrapped:{error.Description}"));

        Assert.True(mapped.IsSuccessful);
        Assert.False(mapped.HasFailed);
        Assert.False(mapped.HasError);
    }

    [Fact]
    public void Fail_WithNullError_HasFailedButNotHasError()
    {
        var result = Result<string?>.Fail(null!);

        Assert.True(result.HasFailed);
        Assert.False(result.HasError);
    }
}
