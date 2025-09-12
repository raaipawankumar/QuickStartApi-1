using System;

namespace QuickStart.Application.Features.Identity;

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsSso { get; set; } = false;

}
