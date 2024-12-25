namespace GameLibrary;

public readonly struct Result
{
    public Exception? Exception { get; }

    public object? Data { get; }

    public bool IsSuccess => Exception is null;


    private Result(object? data)
    {
        Data = data;
    }

    private Result(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Exception = exception;
    }

    public void Predict(Action<object?> success, Action<Exception>? error = null)
    {
        ArgumentNullException.ThrowIfNull(success);
        if (IsSuccess)
            success?.Invoke(Data);
        else
            error?.Invoke(Exception!);
    }
    
    public T1? Predict<T1>(Func<object?, T1> success, Func<Exception, T1>? error = null)
    {
        ArgumentNullException.ThrowIfNull(success);
        if (IsSuccess)
            return success.Invoke(Data);
        return error is null ? default : error.Invoke(Exception!);
    }

    public static Result Success(object? data = null)
    {
        return new Result(data);
    }

    public static Result Fail(Exception exception)
    {
        return new Result(exception);
    }
}

public readonly struct Result<T>
{
    public Exception? Exception { get; }

    public T? Data { get; }

    public bool IsSuccess => Exception is null;


    private Result(T? data)
    {
        Data = data;
    }

    private Result(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Exception = exception;
    }

    public void Predict(Action<object?> success, Action<Exception>? error = null)
    {
        ArgumentNullException.ThrowIfNull(success);
        if (IsSuccess)
            success.Invoke(Data);
        else
            error?.Invoke(Exception!);
    }

    public T1? Predict<T1>(Func<object?, T1> success, Func<Exception, T1>? error = null)
    {
        ArgumentNullException.ThrowIfNull(success);
        if (IsSuccess)
            return success.Invoke(Data);
        return error is null ? default : error.Invoke(Exception!);
    }

    public static Result<T> Success(T? data = default)
    {
        return new Result<T>(data);
    }

    public static Result<T> Fail(Exception exception)
    {
        return new Result<T>(exception);
    }
}

public readonly struct BooleanResult
{
    public Exception? Exception { get; }

    public bool? Data { get; }

    public bool IsTrue => IsSuccess && Data is true;
    public bool IsSuccess => Exception is null;

    private BooleanResult(bool? data)
    {
        Data = data;
    }

    private BooleanResult(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Exception = exception;
    }

    public void Predict(Action<bool?> success, Action<Exception>? error = null)
    {
        ArgumentNullException.ThrowIfNull(success);
        if (IsSuccess)
            success?.Invoke(Data);
        else
            error?.Invoke(Exception!);
    }
    
    public T1? Predict<T1>(Func<bool?, T1> success, Func<Exception, T1>? error = null)
    {
        ArgumentNullException.ThrowIfNull(success);
        if (IsSuccess)
            return success.Invoke(Data);
        return error is null ? default : error.Invoke(Exception!);
    }

    public static BooleanResult True()
    {
        return new BooleanResult(true);
    }
    
    public static BooleanResult False()
    {
        return new BooleanResult(false);
    }

    public static BooleanResult Fail(Exception exception)
    {
        return new BooleanResult(exception);
    }
    
    public static implicit operator bool (BooleanResult result)
    {
        return result.IsTrue;
    }
}