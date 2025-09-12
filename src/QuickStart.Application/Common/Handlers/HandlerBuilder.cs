using FluentValidation;
using Microsoft.Extensions.Logging;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Cache;
using QuickStart.Application.Common.Handlers.Decorators;

namespace QuickStart.Application.Common.Handlers;

public class HandlerBuilder<TRequest>(
IHandler<TRequest> handler,
IHandlerContext handlerContext,
ILogger logger)
{
    private IHandler<TRequest> handler = handler;
    private readonly IHandlerContext handlerContext = handlerContext;
    private ILogger logger = logger;

    public HandlerBuilder<TRequest> WithLogging<TType>(ILogger<TType>? logger = null)
    {
        if (logger is not null)
        {
            this.logger = logger;

        }
        handler = new LoggingDecorator<TRequest>(handler, this.logger);
        return this;
    }

    public HandlerBuilder<TRequest> WithValidator(AbstractValidator<TRequest> validator)
    {
        handler = new ValidationDecorator<TRequest>(handler, validator, logger);
        return this;
    }

    public HandlerBuilder<TRequest> WithCaching<TResposne>(string cacheKey, AppCacheOptions? options = null)
    {
        options ??= AppCacheOptions.Default;
        handler = new CachingDecorator<TRequest, TResposne>(handler, cacheKey, handlerContext.Cache, options, logger);
        return this;
    }
    public HandlerBuilder<TRequest> WithCacheInvalidation(Func<string[]> getCacheKeys)
    {
        handler = new CacheInvalidationDecorator<TRequest>(handler, getCacheKeys, handlerContext.Cache, logger);
        return this;
    }
    public HandlerBuilder<TRequest> WithAudit(IAuditContext auditContext)
    {
        handler = new AuditingDecorator<TRequest>(handler, auditContext, handlerContext, logger);
        return this;
    }


    public IHandler<TRequest> Build()
    {
        return handler;
    }

}
public static class HandlerBuilder
{
     public static HandlerBuilder<TRequest> For<TRequest>(
        IHandler<TRequest> handler, IHandlerContext context, ILogger logger)
    => new(handler, context, logger);
}


