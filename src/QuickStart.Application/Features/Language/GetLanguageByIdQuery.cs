using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Language;


public class GetLanguageByIdQuery(IHandlerContext context) 
: QueryHandler<Guid>(context)
{
    public override async Task<OperationResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
   {

        var existingLanguage = await context.DBRead.Languages
         .SingleOrDefaultAsync(l => (l.Id == id) && (l.Status ?? false),
          cancellationToken);
                      
        if (existingLanguage == null) return new ErrorResult(ErrorResultCode.NotFound);

        return SuccessResult.Create(existingLanguage);
            
      
       
    }
}

