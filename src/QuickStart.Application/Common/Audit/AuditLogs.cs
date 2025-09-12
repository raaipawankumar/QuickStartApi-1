namespace QuickStart.Application.Common.Audit;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
    public string AffectedEntities { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string OldValues { get; set; } = string.Empty;
    public string NewValues { get; set; } = string.Empty;
    public string ChangedFields { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

