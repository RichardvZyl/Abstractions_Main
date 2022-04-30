namespace Abstractions.Results;

public class Result : IResult
{
    protected Result() { }

    public bool Failed => !Succeeded;

    public string Message { get; init; } = string.Empty;

    public bool Succeeded { get; init; }

    public int HttpResponseCode { get; init; }


    #region Fail
    public static IResult Fail()
        => new Result { Succeeded = false, HttpResponseCode = 422 };

    public static IResult Fail(string message, int httpResponseCode = 422)
        => new Result { Succeeded = false, Message = message, HttpResponseCode = httpResponseCode };

    public static Task<IResult> FailAsync()
        => Task.FromResult(Fail());

    public static Task<IResult> FailAsync(string message, int httpResponseCode = 422)
        => Task.FromResult(Fail(message, httpResponseCode));
    #endregion

    #region Success
    public static IResult Success()
        => new Result { Succeeded = true };

    public static IResult Success(string message, int httpResponseCode = 200)
        => new Result { Succeeded = true, Message = message, HttpResponseCode = httpResponseCode };

    public static Task<IResult> SuccessAsync()
        => Task.FromResult(Success());

    public static Task<IResult> SuccessAsync(string message, int httpResponseCode = 200)
        => Task.FromResult(Success(message, httpResponseCode));
    #endregion
}

public class Result<T> : Result, IResult<T>
{
    protected Result() { }

    public T? Data { get; private set; }

    #region Fail
    public static new IResult<T> Fail()
        => new Result<T> { Succeeded = false, HttpResponseCode = 422 };

    public static new IResult<T> Fail(string message, int httpResponseCode = 422)
        => new Result<T> { Succeeded = false, Message = message, HttpResponseCode = httpResponseCode };

    public static new Task<IResult<T>> FailAsync()
        => Task.FromResult(Fail());

    public static new Task<IResult<T>> FailAsync(string message, int httpResponseCode = 422)
        => Task.FromResult(Fail(message, httpResponseCode));
    #endregion


    #region Success
    public static new IResult<T> Success()
        => new Result<T> { Succeeded = true, HttpResponseCode = 200 };

    public static new IResult<T> Success(string message, int httpResponseCode = 200)
        => new Result<T> { Succeeded = true, Message = message, HttpResponseCode = httpResponseCode };

    public static IResult<T> Success(T? data)
        => new Result<T> { Succeeded = true, Data = data, HttpResponseCode = 200 };

    public static IResult<T> Success(T data, string message, int httpResponseCode = 200)
        => new Result<T> { Succeeded = true, Data = data, Message = message, HttpResponseCode = httpResponseCode };

    public static new Task<IResult<T>> SuccessAsync()
        => Task.FromResult(Success());

    public static new Task<IResult<T>> SuccessAsync(string message, int htttpResponseCode = 200)
        => Task.FromResult(Success(message, htttpResponseCode));

    public static Task<IResult<T>> SuccessAsync(T data)
        => Task.FromResult(Success(data));

    public static Task<IResult<T>> SuccessAsync(T data, string message, int httpResponseCode = 200)
        => Task.FromResult(Success(data, message, httpResponseCode));
    #endregion
}
