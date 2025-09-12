using System;

namespace QuickStart.Api.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;

public class SecurityMiddleware(IOptions<SecurityOptions> securityOptions): IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var options = securityOptions.Value;
        ApplySecurityHeaders(context, options);
        ValidateRequestSize(context, options);
        if (options.EnableSqlInjectionProtection)
        {
            ValidateForSqlInjection(context.Request);
        }
        if (options.EnableXssProtection)
        {
            ValidateForXss(context.Request);
        }
        if (options.EnablePathTraversalProtection)
        {
            ValidateForPathTraversal(context.Request);
        }
        
        await next(context);
            
    }
    private static void ApplySecurityHeaders(HttpContext context, SecurityOptions options)
    {
        var response = context.Response;
        var headers = response.Headers;
        
        if (!headers.ContainsKey("Content-Security-Policy"))
        {
            headers.Append("Content-Security-Policy", options.ContentSecurityPolicy);
        }

        if (!headers.ContainsKey("X-Content-Type-Options"))
        {
            headers.Append("X-Content-Type-Options", "nosniff");
        }

        if (!headers.ContainsKey("X-Frame-Options"))
        {
            headers.Append("X-Frame-Options", "DENY");
        }

        if (!headers.ContainsKey("X-XSS-Protection"))
        {
            headers.Append("X-XSS-Protection", "1; mode=block");
        }

        if (context.Request.IsHttps && !headers.ContainsKey("Strict-Transport-Security"))
        {
            headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        }

        if (!headers.ContainsKey("Referrer-Policy"))
        {
            headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
        }

        if (!headers.ContainsKey("Permissions-Policy"))
        {
            headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
        }

        headers.Remove("Server");
        headers.Remove("X-Powered-By");
        headers.Remove("X-AspNet-Version");
    }

    private static void ValidateRequestSize(HttpContext context, SecurityOptions options)
    {
        if (context.Request.ContentLength > options.MaxRequestSize)
        {
            context.Response.StatusCode = 413;
            throw new InvalidOperationException("Request size exceeds limit");
        }
    }

    private static void ValidateForSqlInjection(HttpRequest request)
    {

        var suspiciousPatterns = new[]
        {
            "union select", "drop table", "exec(", "execute(", 
            "sp_", "xp_", "--", "/*", "*/"
        };

        var queryString = request.QueryString.Value?.ToLower() ?? "";
        
        foreach (var pattern in suspiciousPatterns)
        {
            if (queryString.Contains(pattern))
            {
                throw new SecurityException($"Suspicious pattern detected: {pattern}");
            }
        }
    }

    private static void ValidateForXss(HttpRequest request)
    {
        var xssPatterns = new[]
        {
            "<script", "javascript:", "onload=", "onerror=", 
            "onclick=", "onmouseover=", "eval(", "alert("
        };

        var queryString = request.QueryString.Value?.ToLower() ?? "";
        
        foreach (var pattern in xssPatterns)
        {
            if (queryString.Contains(pattern))
            {
                throw new SecurityException($"XSS pattern detected: {pattern}");
            }
        }
    }

    private static void ValidateForPathTraversal(HttpRequest request)
    {
        var path = request.Path.Value?.ToLower() ?? "";
        var traversalPatterns = new[] { "../", "..\\", "%2e%2e", "%c0%af" };

        foreach (var pattern in traversalPatterns)
        {
            if (path.Contains(pattern))
            {
                throw new SecurityException($"Path traversal detected: {pattern}");
            }
        }
    }
   
}

