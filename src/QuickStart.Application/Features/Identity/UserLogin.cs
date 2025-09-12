using System;

namespace QuickStart.Application.Features.Identity;

public class UserLogin
{
 public int ProfileId { get; set; }
 public bool IsLoggedIn { get; set; }
 public bool IsLocked { get; set; }
 public Int16 LoginAttempt { get; set; }
 public DateTime? LoginDateTime { get; set; }
 public string LoginToken { get; set; }
 public DateTime? LoginTokenDate { get; set; }
 public DateTime? LastUpdated { get; set; }
 public int? CompanyId { get; set; }
}
