namespace QuickStart.Application.Common.Handlers;

public static class SuccessResult
{
    public static SuccessResult<Empty> Instance => new(Empty.Instance);
    public static SuccessResult<T?> Create<T>(T? data) => new(data);

}
public class SuccessResult<T>(T data) : OperationResult
{
    public T? Data { get; } = data;
    public override bool IsSuccess => true;

    public override TResult Match<TResult>(
        Func<object, TResult> onSuccess,
        Func<string, List<string>, TResult> onError)
    {
        return onSuccess(Data!);
    }

}


