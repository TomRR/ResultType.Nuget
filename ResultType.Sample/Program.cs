namespace TomRR.ResultType.Sample;

internal static class Program
{
    private static int _checks;

    public static void Main()
    {
        VerifyResultWithoutStatus();
        VerifyResultErrorOnly();
        VerifyResultWithStatus();
        VerifySuccessAndErrorWrappers();
        VerifyBuiltInStatusTypes();
        VerifyCustomStatusType();

        Console.WriteLine($"ResultType.Sample completed successfully with {_checks} checks.");
    }

    private static void VerifyResultWithoutStatus()
    {
        Result<int, Error> successFactory = Result<int, Error>.Success(21);
        Verify(successFactory.IsSuccessful, "Result<TValue, TError>.Success creates success");
        Verify(successFactory.HasValue, "Result<TValue, TError>.Success has value");
        Verify(!successFactory.HasFailed, "Result<TValue, TError>.Success is not failure");
        Verify(!successFactory.HasError, "Result<TValue, TError>.Success has no error");
        Verify(successFactory.Value == 21, "Result<TValue, TError>.Success value is correct");

        var validationError = Error.Validation("invalid number");
        Result<int, Error> failureFactory = Result<int, Error>.Failed(validationError);
        Verify(!failureFactory.IsSuccessful, "Result<TValue, TError>.Failed is not success");
        Verify(!failureFactory.HasValue, "Result<TValue, TError>.Failed has no value");
        Verify(failureFactory.HasFailed, "Result<TValue, TError>.Failed is failure");
        Verify(failureFactory.HasError, "Result<TValue, TError>.Failed has error");
        Verify(failureFactory.Error == validationError, "Result<TValue, TError>.Failed error is correct");

        Result<string, Error> implicitSuccess = "hello";
        Verify(implicitSuccess.IsSuccessful, "Implicit value conversion creates success");
        Verify(implicitSuccess.Value == "hello", "Implicit value conversion keeps value");

        var notFound = Error.NotFound("missing");
        Result<string, Error> implicitFailure = notFound;
        Verify(implicitFailure.HasFailed, "Implicit error conversion creates failure");
        Verify(implicitFailure.Error == notFound, "Implicit error conversion keeps error");

        var successMatch = successFactory.Match(
            onSuccess: value => value.ToString(),
            onFailure: error => error.Description);
        Verify(successMatch == "21", "Match routes success branch");

        var failureMatch = failureFactory.Match(
            onSuccess: value => value.ToString(),
            onFailure: error => error.Description);
        Verify(failureMatch == "invalid number", "Match routes failure branch");

        var mappedSuccess = successFactory.Map(value => value * 2);
        Verify(mappedSuccess.IsSuccessful, "Map transforms success result");
        Verify(mappedSuccess.Value == 42, "Map transforms success value");

        var mappedFailure = failureFactory.Map(value => value * 2);
        Verify(mappedFailure.HasFailed, "Map preserves failure result");
        Verify(mappedFailure.Error == validationError, "Map preserves failure error");

        var boundSuccess = successFactory.Bind(value => Result<string, Error>.Success($"n={value}"));
        Verify(boundSuccess.IsSuccessful, "Bind can chain success");
        Verify(boundSuccess.Value == "n=21", "Bind success value is correct");

        var boundToFailure = successFactory.Bind(value =>
            value > 0
                ? Result<string, Error>.Failed(Error.Conflict("already exists"))
                : Result<string, Error>.Success("unused"));
        Verify(boundToFailure.HasFailed, "Bind can convert success to failure");
        Verify(boundToFailure.Error.ErrorType == ErrorType.Conflict, "Bind failure contains mapped error type");

        var boundFailure = failureFactory.Bind(value => Result<string, Error>.Success($"n={value}"));
        Verify(boundFailure.HasFailed, "Bind preserves failure");
        Verify(boundFailure.Error == validationError, "Bind preserves original failure error");

        var mapErrorFailure = failureFactory.MapError(error => Error.Unexpected($"wrapped:{error.Description}"));
        Verify(mapErrorFailure.HasFailed, "MapError transforms failure");
        Verify(mapErrorFailure.Error.ErrorType == ErrorType.Unexpected, "MapError changes error type");
        Verify(mapErrorFailure.Error.Description == "wrapped:invalid number", "MapError changes error description");

        var mapErrorSuccess = successFactory.MapError(error => Error.Unexpected(error.Description));
        Verify(mapErrorSuccess.IsSuccessful, "MapError preserves success");
        Verify(mapErrorSuccess.Value == 21, "MapError keeps success value");

        var nullableSuccess = Result<string?, Error>.Success(null!);
        Verify(nullableSuccess.IsSuccessful, "Null success is still success state");
        Verify(!nullableSuccess.HasValue, "Null success does not have value");
    }

    private static void VerifyResultWithStatus()
    {
        Result<int, Error, NoContent> success = Result<int, Error, NoContent>.Success(12);
        Verify(success.IsSuccessful, "Status result success factory creates success");
        Verify(success.HasValue, "Status result success has value");
        Verify(!success.HasFailed, "Status result success is not failed");
        Verify(!success.IsStatusOnly, "Status result success is not status-only");
        Verify(success.Value == 12, "Status result success value is correct");

        var failureError = Error.Failure("failed operation");
        Result<int, Error, NoContent> failure = Result<int, Error, NoContent>.Failed(failureError);
        Verify(!failure.IsSuccessful, "Status result failure is not success");
        Verify(failure.HasFailed, "Status result failure is failed");
        Verify(failure.HasError, "Status result failure has error");
        Verify(!failure.IsStatusOnly, "Status result failure is not status-only");
        Verify(failure.Error == failureError, "Status result failure error is correct");

        Result<int, Error, NoContent> status = Result<int, Error, NoContent>.Status(Result.NoContent);
        Verify(!status.IsSuccessful, "Status factory result is not success");
        Verify(!status.HasFailed, "Status factory result is not failure");
        Verify(status.IsStatusOnly, "Status factory result is status-only");
        Verify(status.HasStatusOnly, "Status factory result has status value");

        Result<string, Error, Accepted> implicitValue = "payload";
        Verify(implicitValue.IsSuccessful, "Status generic implicit success works");

        var implicitErrorValue = Error.Validation("validation failed");
        Result<string, Error, Accepted> implicitError = implicitErrorValue;
        Verify(implicitError.HasFailed, "Status generic implicit failure works");
        Verify(implicitError.Error == implicitErrorValue, "Status generic implicit failure carries error");

        Result<string, Error, Accepted> implicitStatus = Result.Accepted;
        Verify(implicitStatus.IsStatusOnly, "Status generic implicit status works");

        var successMatch = success.Match(
            onSuccess: value => $"success:{value}",
            onFailure: error => $"failure:{error.Description}",
            onStatusOnly: _ => "status");
        Verify(successMatch == "success:12", "Status result Match routes success");

        var failureMatch = failure.Match(
            onSuccess: value => $"success:{value}",
            onFailure: error => $"failure:{error.Description}",
            onStatusOnly: _ => "status");
        Verify(failureMatch == "failure:failed operation", "Status result Match routes failure");

        var statusMatch = status.Match(
            onSuccess: value => $"success:{value}",
            onFailure: error => $"failure:{error.Description}",
            onStatusOnly: _ => "status");
        Verify(statusMatch == "status", "Status result Match routes status-only");

        var mappedSuccess = success.Map(value => value * 10);
        Verify(mappedSuccess.IsSuccessful, "Status result Map transforms success");
        Verify(mappedSuccess.Value == 120, "Status result Map success value is correct");

        var mappedFailure = failure.Map(value => value * 10);
        Verify(mappedFailure.HasFailed, "Status result Map preserves failure");
        Verify(mappedFailure.Error == failureError, "Status result Map preserves failure error");

        var mappedStatus = status.Map(value => value * 10);
        Verify(mappedStatus.IsStatusOnly, "Status result Map preserves status-only");

        var boundSuccess = success.Bind(value => Result<string, Error, NoContent>.Success($"ok:{value}"));
        Verify(boundSuccess.IsSuccessful, "Status result Bind chains success");
        Verify(boundSuccess.Value == "ok:12", "Status result Bind success value is correct");

        var boundToFailure = success.Bind(value =>
            value > 0
                ? Result<string, Error, NoContent>.Failed(Error.Conflict("cannot process"))
                : Result<string, Error, NoContent>.Success("unused"));
        Verify(boundToFailure.HasFailed, "Status result Bind can return failure");
        Verify(boundToFailure.Error.ErrorType == ErrorType.Conflict, "Status result Bind failure type is correct");

        var boundFailure = failure.Bind(value => Result<string, Error, NoContent>.Success($"ok:{value}"));
        Verify(boundFailure.HasFailed, "Status result Bind preserves failure");
        Verify(boundFailure.Error == failureError, "Status result Bind keeps failure error");

        var boundStatus = status.Bind(value => Result<string, Error, NoContent>.Success($"ok:{value}"));
        Verify(boundStatus.IsStatusOnly, "Status result Bind preserves status-only");

        var mapErrorFailure = failure.MapError(error => Error.NotFound($"wrapped:{error.Description}"));
        Verify(mapErrorFailure.HasFailed, "Status result MapError transforms failure");
        Verify(mapErrorFailure.Error.ErrorType == ErrorType.NotFound, "Status result MapError type is correct");
        Verify(mapErrorFailure.Error.Description == "wrapped:failed operation", "Status result MapError description is correct");

        var mapErrorSuccess = success.MapError(error => Error.NotFound(error.Description));
        Verify(mapErrorSuccess.IsSuccessful, "Status result MapError preserves success");
        Verify(mapErrorSuccess.Value == 12, "Status result MapError keeps success value");

        var mapErrorStatus = status.MapError(error => Error.NotFound(error.Description));
        Verify(mapErrorStatus.IsStatusOnly, "Status result MapError preserves status-only");

        var nullableSuccess = Result<string?, Error, NoContent>.Success(null!);
        Verify(nullableSuccess.IsSuccessful, "Status result null success is still success state");
        Verify(!nullableSuccess.HasValue, "Status result null success has no value");
    }

    private static void VerifyResultErrorOnly()
    {
        var ok = Result<Error>.Ok();
        Verify(ok.IsSuccessful, "Result<TError>.Ok is successful");
        Verify(ok.IsSuccess, "Result<TError>.IsSuccess alias is true for Ok");
        Verify(!ok.HasFailed, "Result<TError>.Ok is not failed");
        Verify(!ok.HasError, "Result<TError>.Ok has no error");

        var failureError = Error.Validation("invalid command");
        var fail = Result<Error>.Fail(failureError);
        Verify(!fail.IsSuccessful, "Result<TError>.Fail is not successful");
        Verify(!fail.IsSuccess, "Result<TError>.IsSuccess alias is false for Fail");
        Verify(fail.HasFailed, "Result<TError>.Fail is failed");
        Verify(fail.HasError, "Result<TError>.Fail has error");
        Verify(fail.Error == failureError, "Result<TError>.Fail keeps error value");

        Result<Error> implicitFailure = Error.Conflict("already exists");
        Verify(implicitFailure.HasFailed, "Result<TError> implicit conversion creates failure");
        Verify(implicitFailure.HasError, "Result<TError> implicit conversion has error");
        Verify(implicitFailure.Error.ErrorType == ErrorType.Conflict, "Result<TError> implicit error type is correct");

        var okMessage = ok.Match(
            onSuccess: () => "ok",
            onFailure: error => error.Description);
        Verify(okMessage == "ok", "Result<TError>.Match routes success branch");

        var failMessage = fail.Match(
            onSuccess: () => "ok",
            onFailure: error => error.Description);
        Verify(failMessage == "invalid command", "Result<TError>.Match routes failure branch");

        var mappedFail = fail.MapError(error => Error.Unexpected($"wrapped:{error.Description}"));
        Verify(mappedFail.HasFailed, "Result<TError>.MapError preserves failure state");
        Verify(mappedFail.HasError, "Result<TError>.MapError failure has error");
        Verify(mappedFail.Error.ErrorType == ErrorType.Unexpected, "Result<TError>.MapError updates error type");
        Verify(mappedFail.Error.Description == "wrapped:invalid command", "Result<TError>.MapError updates error description");

        var mappedOk = ok.MapError(error => Error.Unexpected(error.Description));
        Verify(mappedOk.IsSuccessful, "Result<TError>.MapError preserves success state");
        Verify(!mappedOk.HasError, "Result<TError>.MapError on success keeps no error");

        var nullFailure = Result<string?>.Fail(null!);
        Verify(nullFailure.HasFailed, "Result<TError> null fail remains failed state");
        Verify(!nullFailure.HasError, "Result<TError> null fail has no non-null error");
    }

    private static void VerifySuccessAndErrorWrappers()
    {
        var successViaFactory = Success<string>.Of("saved");
        Verify(successViaFactory.IsSuccessful, "Success<T>.Of creates success");
        Verify(successViaFactory.HasValue, "Success<T>.Of has value");
        Verify(successViaFactory.Value == "saved", "Success<T>.Of value is correct");

        Success<int> successImplicit = 42;
        Verify(successImplicit.IsSuccessful, "Success<T> implicit conversion works");
        Verify(successImplicit.Value == 42, "Success<T> implicit conversion keeps value");

        var successViaResultFactory = Result.SuccessOf("ok");
        Verify(successViaResultFactory.IsSuccessful, "Result.SuccessOf creates Success<T>");
        Verify(successViaResultFactory.Value == "ok", "Result.SuccessOf value is correct");

        var errorViaFactory = Error<string>.Of("bad request");
        Verify(errorViaFactory.IsFailed, "Error<T>.Of creates error");
        Verify(errorViaFactory.HasError, "Error<T>.Of has error");
        Verify(errorViaFactory.Value == "bad request", "Error<T>.Of value is correct");

        Error<int> errorImplicit = 500;
        Verify(errorImplicit.IsFailed, "Error<T> implicit conversion works");
        Verify(errorImplicit.Value == 500, "Error<T> implicit conversion keeps value");

        var errorViaResultFactory = Result.ErrorOf("invalid");
        Verify(errorViaResultFactory.IsFailed, "Result.ErrorOf creates Error<T>");
        Verify(errorViaResultFactory.Value == "invalid", "Result.ErrorOf value is correct");

        var errorFromCreate = Error.Create("custom", ErrorType.Conflict);
        Verify(errorFromCreate.ErrorType == ErrorType.Conflict, "Error.Create sets type");
        Verify(errorFromCreate.Description == "custom", "Error.Create sets description");

        Verify(Error.Failure().ErrorType == ErrorType.Failure, "Error.Failure sets Failure type");
        Verify(Error.Unexpected().ErrorType == ErrorType.Unexpected, "Error.Unexpected sets Unexpected type");
        Verify(Error.Validation().ErrorType == ErrorType.Validation, "Error.Validation sets Validation type");
        Verify(Error.Conflict().ErrorType == ErrorType.Conflict, "Error.Conflict sets Conflict type");
        Verify(Error.NotFound().ErrorType == ErrorType.NotFound, "Error.NotFound sets NotFound type");
    }

    private static void VerifyBuiltInStatusTypes()
    {
        VerifyStatusType(Result.NoContent, nameof(NoContent));
        VerifyStatusType(Result.NotModified, nameof(NotModified));
        VerifyStatusType(Result.Success, nameof(Success));
        VerifyStatusType(Result.Created, nameof(Created));
        VerifyStatusType(Result.Accepted, nameof(Accepted));
        VerifyStatusType(Result.Deleted, nameof(Deleted));
        VerifyStatusType(Result.Updated, nameof(Updated));
        VerifyStatusType(Result.Skipped, nameof(Skipped));
        VerifyStatusType(Result.Timeout, "Timeout");
        VerifyStatusType(Result.Cancelled, nameof(Cancelled));
        VerifyStatusType(Result.Retried, nameof(Retried));
        VerifyStatusType(Result.None, nameof(None));
        VerifyStatusType(Result.Empty, nameof(Empty));
    }

    private static void VerifyCustomStatusType()
    {
        Result<string, Error, Deferred> statusResult = Result<string, Error, Deferred>.Status(new Deferred());
        Verify(statusResult.IsStatusOnly, "Custom status can be used as status-only branch");

        var output = statusResult.Match(
            onSuccess: value => value,
            onFailure: error => error.Description,
            onStatusOnly: _ => "deferred");

        Verify(output == "deferred", "Custom status is routed in Match");

        Verify(Marshal.SizeOf<Deferred>() == 1, "Custom status struct keeps size 1");
    }

    private static void VerifyStatusType<TStatus>(TStatus status, string name)
        where TStatus : struct, IStatusOnlyResult
    {
        Verify(typeof(Unit).IsAssignableFrom(typeof(TStatus)), $"{name} implements Unit");
        Verify(Marshal.SizeOf<TStatus>() == 1, $"{name} struct size is 1");

        Result<int, Error, TStatus> result = status;
        Verify(result.IsStatusOnly, $"{name} can be converted to status-only result");

        var matched = result.Match(
            onSuccess: value => value.ToString(),
            onFailure: error => error.Description,
            onStatusOnly: _ => name);

        Verify(matched == name, $"{name} routes to status-only Match branch");
    }

    private static void Verify(bool condition, string message)
    {
        _checks++;
        if (!condition)
        {
            throw new InvalidOperationException($"Verification failed: {message}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private readonly struct Deferred : IStatusOnlyResult;
}
