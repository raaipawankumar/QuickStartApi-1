using System;

namespace QuickStart.Application.Features.Role;

public record GetRoleByIdRequest
{
    public int Id { get; set; }
    public bool IncludeRights { get; set; } = false;

}
