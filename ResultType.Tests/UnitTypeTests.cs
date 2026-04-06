namespace TomRR.ResultType.Tests;

public class UnitTypeTests
{
    [Theory]
    [InlineData(typeof(NoContent))]
    [InlineData(typeof(NotModified))]
    [InlineData(typeof(Success))]
    [InlineData(typeof(Created))]
    [InlineData(typeof(Accepted))]
    [InlineData(typeof(Deleted))]
    [InlineData(typeof(Updated))]
    [InlineData(typeof(Skipped))]
    [InlineData(typeof(ResultType.UnitTypes.Timeout))]
    [InlineData(typeof(Cancelled))]
    [InlineData(typeof(Retried))]
    [InlineData(typeof(None))]
    [InlineData(typeof(Empty))]
    public void AllUnitTypes_ImplementIStatusOnlyResult(Type unitType)
    {
        Assert.True(typeof(IStatusOnlyResult).IsAssignableFrom(unitType));
    }

    [Theory]
    [InlineData(typeof(NoContent))]
    [InlineData(typeof(NotModified))]
    [InlineData(typeof(Success))]
    [InlineData(typeof(Created))]
    [InlineData(typeof(Accepted))]
    [InlineData(typeof(Deleted))]
    [InlineData(typeof(Updated))]
    [InlineData(typeof(Skipped))]
    [InlineData(typeof(ResultType.UnitTypes.Timeout))]
    [InlineData(typeof(Cancelled))]
    [InlineData(typeof(Retried))]
    [InlineData(typeof(None))]
    [InlineData(typeof(Empty))]
    public void AllUnitTypes_ImplementUnit(Type unitType)
    {
        Assert.True(typeof(Unit).IsAssignableFrom(unitType));
    }

    [Fact]
    public void ResultFactory_NoContent_ReturnsInstance()
    {
        var value = Result.NoContent;
        Assert.IsType<NoContent>(value);
    }

    [Fact]
    public void ResultFactory_NotModified_ReturnsInstance()
    {
        var value = Result.NotModified;
        Assert.IsType<NotModified>(value);
    }

    [Fact]
    public void ResultFactory_Success_ReturnsInstance()
    {
        var value = Result.Success;
        Assert.IsType<Success>(value);
    }

    [Fact]
    public void ResultFactory_Created_ReturnsInstance()
    {
        var value = Result.Created;
        Assert.IsType<Created>(value);
    }

    [Fact]
    public void ResultFactory_Accepted_ReturnsInstance()
    {
        var value = Result.Accepted;
        Assert.IsType<Accepted>(value);
    }

    [Fact]
    public void ResultFactory_Deleted_ReturnsInstance()
    {
        var value = Result.Deleted;
        Assert.IsType<Deleted>(value);
    }

    [Fact]
    public void ResultFactory_Updated_ReturnsInstance()
    {
        var value = Result.Updated;
        Assert.IsType<Updated>(value);
    }

    [Fact]
    public void ResultFactory_Skipped_ReturnsInstance()
    {
        var value = Result.Skipped;
        Assert.IsType<Skipped>(value);
    }

    [Fact]
    public void ResultFactory_Timeout_ReturnsInstance()
    {
        var value = Result.Timeout;
        Assert.IsType<ResultType.UnitTypes.Timeout>(value);
    }

    [Fact]
    public void ResultFactory_Cancelled_ReturnsInstance()
    {
        var value = Result.Cancelled;
        Assert.IsType<Cancelled>(value);
    }

    [Fact]
    public void ResultFactory_Retried_ReturnsInstance()
    {
        var value = Result.Retried;
        Assert.IsType<Retried>(value);
    }

    [Fact]
    public void ResultFactory_None_ReturnsInstance()
    {
        var value = Result.None;
        Assert.IsType<None>(value);
    }

    [Fact]
    public void ResultFactory_Empty_ReturnsInstance()
    {
        var value = Result.Empty;
        Assert.IsType<Empty>(value);
    }

    [Fact]
    public void ResultFactory_SuccessOf_CreatesSuccess()
    {
        var success = Result.SuccessOf(42);

        Assert.True(success.IsSuccessful);
        Assert.Equal(42, success.Value);
    }

    [Fact]
    public void ResultFactory_ErrorOf_CreatesError()
    {
        var error = Result.ErrorOf("bad");

        Assert.True(error.IsFailed);
        Assert.Equal("bad", error.Value);
    }
}
