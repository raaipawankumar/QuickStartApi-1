namespace QuickStart.Application.Common.Handlers;

public class ErrorResult : OperationResult
{
    public override bool IsSuccess => false;

    public string ErrorMessage = string.Empty;

    public List<string> ValidationErrors { get; } = [];

    public ErrorResult(string error)
    {
        ValidationErrors.Add(error);

    }
    public ErrorResult(List<string> errors)
    {
        ValidationErrors = errors;
    }
    public override TResult Match<TResult>(
        Func<object, TResult> onSuccess,
        Func<string, List<string>, TResult> onError)
    {
        return onError(ErrorMessage, ValidationErrors);
    }
    public static ErrorResult Create(string error) => new(error);
    public static ErrorResult Create(List<string> errors) => new(errors);
}

