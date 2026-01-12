// using Business.Exceptions; // Temporarily disabled
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            
            // Ensure CORS headers are added even when exception occurs
            if (!context.Response.HasStarted)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Don't modify response if it has already started
        if (context.Response.HasStarted)
        {
            return Task.CompletedTask;
        }
        
        context.Response.ContentType = "application/json";

        int statusCode;
        object response;

        // Simplified exception handling - custom exceptions temporarily disabled
        statusCode = (int)HttpStatusCode.InternalServerError;
        response = new
        {
            statusCode,
            message = exception.Message ?? "An internal server error occurred. Please try again later.",
            type = exception.GetType().Name
        };

        context.Response.StatusCode = statusCode;
        
        // Add CORS headers manually if not already present
        if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
        {
            context.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:5175";
        }
        if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Credentials"))
        {
            context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
        }
        
        return context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}

public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
