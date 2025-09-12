using QuickStart.Application.Features.Right;

namespace QuickStart.Application.Features.Role;

public record GetRoleByIdResponse
{
    public Role Role { get; set; } = new Role();
    public RoleRight[] Rights { get; set; } = [];

}
