using System.Data;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Language;

public class GetAllLanguages(IHandlerContext context) : QueryHandler<NoInput>(context)
{


    public override async Task<OperationResult> HandleAsync(NoInput request, CancellationToken cancellationToken = default)
    {

         var languages = await context.DBRead.Languages
            .Where(l => l.Status ?? false)
            .ToArrayAsync(cancellationToken);

          return new SuccessResult<Language[]>(languages);
        }
  
    }

