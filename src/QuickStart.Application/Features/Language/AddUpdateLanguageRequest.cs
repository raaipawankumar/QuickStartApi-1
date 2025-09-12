using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Language;

public class AddUpdateLanguageRequest : IAppCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; } = string.Empty;
    public string? Description { get; set; } 
    public int? CreatedBy { get; set; }
    public bool Status { get; set; } = true;
    public bool? IsDefault { get; set; } = false;
}
