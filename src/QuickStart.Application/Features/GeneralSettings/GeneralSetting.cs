namespace QuickStart.Application.Features.General
{
    public class GeneralSetting
    {
        public int? Id { get; set; }

        public string? SettingKey { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? Type { get; set; }
        public string? Detail { get;  set; } 
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
    }
}
