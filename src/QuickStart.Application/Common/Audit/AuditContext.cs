namespace QuickStart.Application.Common.Audit;

public interface IAuditContext
{
    string GetCurrentUserId();
    string GetCurrentUserName();
    string GetIpAddress();
    string GetUserAgent();
    string GetCorrelationId();
    public EntityAuditDetail EntityDetail { get; set; }
}


