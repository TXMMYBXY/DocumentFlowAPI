using System.Net;
using System.Text.Json;

namespace DocumentFlowAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        HttpStatusCode errorCode;
        string message;
        try
        {
            await _next(context);
            return;
        }
        catch (InvalidOperationException ex) //409
        {
            errorCode = HttpStatusCode.Conflict;
            message = ex.Message;
        }
        catch (ArgumentException ex) //400
        {
            errorCode = HttpStatusCode.BadRequest;
            message = ex.Message;
        }
        catch (NullReferenceException ex) //404
        {
            errorCode = HttpStatusCode.NotFound;
            message = ex.Message;
        }
        catch (Exception ex) //500
        {
            errorCode = HttpStatusCode.InternalServerError;
            message = ex.Message;
        }
        await _HandleExceptionAsync(context, message, errorCode);
    }
    
    private static async Task _HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                Message = message,
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
}
public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}