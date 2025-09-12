namespace QuickStart.Application.Features.Language;

public class Language
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int? CreatedBy { get; set; }
    public bool? Status { get; set; }
    public bool? IsDefault { get; set; }
   // public bool IsDeleted { get; set; } = false;

}
