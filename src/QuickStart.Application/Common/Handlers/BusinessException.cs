using System.Text;

namespace QuickStart.Application.Common.Handlers;

[Serializable]
public class BusinessException : Exception
{
    public string? ErrorCode { get; }
    public DateTime OccurredAt { get; }
    public Dictionary<string, object> Context { get; }

    public BusinessException() : base()
    {
        OccurredAt = DateTime.UtcNow;
        Context = [];
    }

    public BusinessException(string message) : base(message)
    {
        OccurredAt = DateTime.UtcNow;
        Context = [];
    }

    public BusinessException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
        OccurredAt = DateTime.UtcNow;
        Context = [];
    }

    public BusinessException(string message, string errorCode, string businessRule) 
        : base(message)
    {
        ErrorCode = errorCode;
        OccurredAt = DateTime.UtcNow;
        Context = [];
    }

    public BusinessException(string message, Exception innerException) 
        : base(message, innerException)
    {
        OccurredAt = DateTime.UtcNow;
        Context = [];
    }

    public BusinessException(string message, string errorCode, Exception innerException) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        OccurredAt = DateTime.UtcNow;
        Context = [];
    }

    public BusinessException AddContext(string key, object value)
    {
        Context[key] = value;
        return this;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"BusinessException: {Message}");
        
        if (!string.IsNullOrEmpty(ErrorCode))
            sb.AppendLine($"Error Code: {ErrorCode}");
            
                 
        sb.AppendLine($"Occurred At: {OccurredAt:yyyy-MM-dd HH:mm:ss} UTC");
        
        if (Context.Count != 0)
        {
            sb.AppendLine("Context:");
            foreach (var kvp in Context)
            {
                sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }
        }
        
        if (InnerException != null)
        {
            sb.AppendLine($"Inner Exception: {InnerException}");
        }
        
        sb.AppendLine($"Stack Trace: {StackTrace}");
        
        return sb.ToString();
    }
}

