namespace QuickStart.Application.Features.Role;

public class AddOrUpdateRoleRequest
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<int> Rights { get; set; } = [];

}
