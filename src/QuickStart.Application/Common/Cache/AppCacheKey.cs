namespace QuickStart.Application.Common.Cache;

public static class AppCacheKey
{
    
    private static readonly string Prefix = "smartwx-admin";
    public static readonly string AllGeneralSettings = $"{Prefix}-settings-general-all";
    public static class Language
    {
        public static readonly string AllLanguages = $"{Prefix}-languages-all";
    }
    public static class Role
    {
        public static readonly string AllRolesWithSummary = $"{Prefix}-roles-all";
    }
    
}
