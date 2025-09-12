using System;

namespace QuickStart.Application.Features.Identity.Role;

public class UserRole
{
    public int UserRoleId { get; set; }
    public int ProfileId { get; set; }
    public int RoleId { get; set; }
    public bool IsActive{ get; set; }
}
