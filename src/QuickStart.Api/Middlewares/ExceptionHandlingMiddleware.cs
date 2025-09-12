using System.Net;
using System.Text.Json;

namespace QuickStart.Api.Middlewares;

public class ExceptionHandlingMiddleware(
  ILogger<ExceptionHandlingMiddleware> logger)
    : IMiddleware
{

    private readonly ILogger<ExceptionHandlingMiddleware> logger = logger;

    private const string StructuredExceptionLogFormat = @"[{CorrelationId}]: 
    Exception occurred during request processing. " +
            "Path: {Path}, Method: {Method}, QueryString: {QueryString}, " +
            "UserAgent: {UserAgent}, RemoteIpAddress: {RemoteIpAddress}";
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.Request.Headers[AddCorreltationIdMiddleware.XCorrelationId];
        logger.LogError(exception, StructuredExceptionLogFormat,
            correlationId,
            context.Request.Path,
            context.Request.Method,
            context.Request.QueryString,
            context.Request.Headers.UserAgent.FirstOrDefault(),
            context.Connection.RemoteIpAddress?.ToString());

        var response = CreateErrorResponse(exception, correlationId);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

       
        var jsonResponse = JsonSerializer.Serialize(response, jsonSerializerOptions);

        await context.Response.WriteAsync(jsonResponse);
    }

    private static ApiErrorResponse CreateErrorResponse(Exception exception, string correlationId)
    {
        return exception switch
        {
           
            ArgumentException => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Bad request",
                Detail = exception.Message,
                CorrelationId = correlationId
            },
            UnauthorizedAccessException => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Unauthorized access",
                Detail = "You are not authorized to perform this action",
                CorrelationId = correlationId
            },
            KeyNotFoundException => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Resource not found",
                Detail = exception.Message,
                CorrelationId = correlationId
            },
            _ => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Internal server error",
                Detail = "An unexpected error occurred",
                CorrelationId = correlationId
            }
        };
    }
   
}
