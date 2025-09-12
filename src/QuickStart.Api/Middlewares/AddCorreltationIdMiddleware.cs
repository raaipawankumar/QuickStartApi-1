namespace QuickStart.Api.Middlewares;

public class AddCorreltationIdMiddleware : IMiddleware
{
    public const string XCorrelationId = "X-Correlation-ID";
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var id = $"request-id-{Guid.NewGuid()}";
        if (context.Request.Headers.ContainsKey(XCorrelationId))
        {
            context.Request.Headers[XCorrelationId] = id;
        }
        else
        {
            context.Request.Headers.Append(XCorrelationId, id);
        }
       await next(context);
    }
}
