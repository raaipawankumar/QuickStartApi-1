using System;
using QuickStart.Application.Common.Audit;

namespace QuickStart.Api.Common;

public class AuditContext(IHttpContextAccessor httpContextAccessor) : IAuditContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public EntityAuditDetail EntityDetail { get; set; } = new EntityAuditDetail();

    public string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value 
               ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name 
               ?? "System";

    }

    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
    }

    public string GetIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.Request.Headers["X-Forwarded-For"].FirstOrDefault() 
               ?? context?.Connection.RemoteIpAddress?.ToString() 
               ?? "Unknown";
    }

    public string GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].FirstOrDefault() 
               ?? "Unknown";
    }

    public string GetCorrelationId()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-ID"].FirstOrDefault() 
               ?? Guid.NewGuid().ToString();
    }
}
