using System.Text.Json;
using Microsoft.Extensions.Logging;
using QuickStart.Application.Common.Audit;

namespace QuickStart.Application.Common.Handlers.Decorators;

public class AuditingDecorator<TRequest>(
    IHandler<TRequest> innerHandler,
    IAuditContext auditContext,
    IHandlerContext context,
    ILogger logger)
     : HandlerDecorator<TRequest>(innerHandler)
{

    public override Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var result = innerHandler.HandleAsync(request, cancellationToken);
        if (request is not IAppCommand)
        {
            logger.LogInformation("Skipping audit logging for non command action");
            return result;
        }
        var auditLog = CreateAuditLogFromAuditContext(auditContext);
        logger.LogInformation("Added audit detail for {action}", auditLog.Action);
        logger.LogInformation("Audit {detail}",JsonSerializer.Serialize(auditLog));
        return result;
    }
    private static AuditLog CreateAuditLogFromAuditContext(IAuditContext context) {
        return new AuditLog
        {
            Id = Guid.NewGuid(),
            CorrelationId = context.GetCorrelationId(),
            Timestamp = DateTime.UtcNow,
            IpAddress = context.GetIpAddress(),
            UserAgent = context.GetUserAgent(),
            UserId = context.GetCurrentUserId(),
            UserName = context.GetCurrentUserName(),
            AffectedEntities = context.EntityDetail.AffectedEntities,
            Action = context.EntityDetail.Action,

            OldValues = context.EntityDetail.OldValues,
            NewValues = context.EntityDetail.NewValues,
            ChangedFields = context.EntityDetail.ChangedFields,
        };
    }
}



