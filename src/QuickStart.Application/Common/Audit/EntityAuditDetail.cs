namespace QuickStart.Application.Common.Audit;

public class EntityAuditDetail
{
    public string AffectedEntities { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string OldValues { get; set; } = string.Empty;
    public string NewValues { get; set; } = string.Empty;
    public string ChangedFields { get; set; } = string.Empty;
}

