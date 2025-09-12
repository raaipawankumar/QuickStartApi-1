using System.Text;

namespace QuickStart.Application.Common.Audit;

public class AuditAffectedEntity
{
    private readonly Dictionary<string, List<string>> affected = [];
    private AuditAffectedEntity()
    {

    }
    public AuditAffectedEntity Add(string entityName, string entityId)
    {
        if (affected.TryGetValue(entityName, out List<string>? value))
        {
            value.Add(entityId);
        }
        else
        {
            affected.Add(entityName, [entityId]);
        }
        return this;
    }
    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var key in affected.Keys)
        {
            builder.Append($"{key}:{string.Join(',', affected[key])}|");
        }
        return builder.ToString();
    }
    public static AuditAffectedEntity Instance => new();
}

