namespace QuickStart.Application.Features.Role;

public class Role
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public DateTime LastUpdated { get; set; }

}
