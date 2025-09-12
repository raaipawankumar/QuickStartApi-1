namespace QuickStart.Api.Middlewares;

public class SecurityOptions
{
    public const string SectionName = "Security";
    public string ContentSecurityPolicy { get; set; } =
        "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' https:";
    
    public long MaxRequestSize { get; set; } = 10 * 1024 * 1024; // 10MB
    
    public bool EnableSqlInjectionProtection { get; set; } = true;
    
    public bool EnableXssProtection { get; set; } = true;
    
    public bool EnablePathTraversalProtection { get; set; } = true;
}

