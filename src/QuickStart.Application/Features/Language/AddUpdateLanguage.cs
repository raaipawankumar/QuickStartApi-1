using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Data;
using QuickStart.Application.Data.Extensions;

namespace QuickStart.Application.Features.Language;

public class AddUpdateLanguage(IHandlerContext context, IAuditContext auditContext) 
: CommandHandler<AddUpdateLanguageRequest>(context, auditContext)
{
    private Language? existingLanguage;
    public override async Task<OperationResult> HandleAsync(AddUpdateLanguageRequest request,
        CancellationToken cancellationToken = default)
    {

        existingLanguage = await context.DBWrite.Languages
            .SingleOrDefaultAsync(l => (l.Id == request.Id) && (l.Status ?? false),
             cancellationToken);
        


        await (existingLanguage == null
            ? AddLanguageAsync(context.DBWrite, request, cancellationToken)
            : UpdateLanguageAsync(context.DBWrite, existingLanguage, request, cancellationToken));
        
        
        return SuccessResult.Instance;

    }
    private async static Task AddLanguageAsync(
        SmartWxWriteOnlyContext db,
         AddUpdateLanguageRequest request,
     CancellationToken cancellationToken)
    {
        db.Languages.Add(new Language
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            CreatedBy = request.CreatedBy,
            Status = request.Status,
            IsDefault = request.IsDefault
        });
        await db.SaveChangesAsync(cancellationToken);
    }
    private async static Task UpdateLanguageAsync(SmartWxWriteOnlyContext db,
     Language existing,
     AddUpdateLanguageRequest request,
      CancellationToken cancellationToken)
    {
        existing.Code = request.Code;
        existing.Name = request.Name;
        existing.Description = request.Description;
        existing.IsDefault = request.IsDefault;
        existing.Status = request.Status;
        db.Languages.Update(existing);
        await db.SaveChangesAsync(cancellationToken);
    }

    protected override void SetAuditDetail(AddUpdateLanguageRequest request)
    {
        var affectedId = existingLanguage is null ? request.Id : existingLanguage.Id;
        var affectedEntities = AuditAffectedEntity.Instance
            .Add(context.DBWrite.GetTableName<Language>(), affectedId.ToString())
            .ToString();

        auditContext.EntityDetail = new EntityAuditDetail
        {
            Action = nameof(AddUpdateLanguage),
            OldValues = existingLanguage is null ? JsonSerializer.Serialize(existingLanguage) : "{}",
            NewValues = JsonSerializer.Serialize(request),
            AffectedEntities = affectedEntities
        };
    }



}
