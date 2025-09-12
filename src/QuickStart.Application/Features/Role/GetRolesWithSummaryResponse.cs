using System;

namespace QuickStart.Application.Features.Role;

public class GetRolesWithSummaryResponse
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int TotalUsers { get; set; }
    public int TotalRights { get; set; }
    public bool Status { get; set; }
    public DateTime LastUpdated { get; set; }

}
