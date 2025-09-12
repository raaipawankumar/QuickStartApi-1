using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Features.General;

namespace QuickStart.Application.Features.GeneralSettings;

public class GetGeneralSettings(IHandlerContext context) : QueryHandler<NoInput>(context)
{
    public override async Task<OperationResult> HandleAsync(NoInput request,
        CancellationToken cancellationToken = default)
    {
        var settings = await context.DBRead.GeneralSettings
            .Where(gs => gs.IsActive ?? false)
            .ToArrayAsync(cancellationToken);
     
        return new SuccessResult<GeneralSetting[]>(settings);
    }

  
}
