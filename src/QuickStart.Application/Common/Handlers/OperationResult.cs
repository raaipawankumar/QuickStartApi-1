namespace QuickStart.Application.Common.Handlers;

public abstract class OperationResult
{
    public abstract bool IsSuccess { get; }
    protected readonly Dictionary<string, string> AdditionalData = [];
    public string GetAdditionalData(string key) => AdditionalData[key];
    public abstract TResult Match<TResult>(
        Func<object, TResult> onSuccess,
        Func<string, List<string>, TResult> onError);
}

