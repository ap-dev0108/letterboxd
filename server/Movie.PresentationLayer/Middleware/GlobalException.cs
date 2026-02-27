using System.Net;
using System.Text.Json;

namespace Movie.Presentation.Middleware.Global;

public class GlobalException
{
    private readonly RequestDelegate _nextRequest;

    public GlobalException(RequestDelegate nextRequest)
    {
        _nextRequest = nextRequest;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _nextRequest(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var status = ex switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            BadHttpRequestException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            InvalidOperationException => HttpStatusCode.Conflict,
            
            _ => HttpStatusCode.InternalServerError
        };

        var response = new
        {
            success = false,
            message = ex.Message,
            error = ex.GetType().Name,
            traceID = context.TraceIdentifier,
            path = context.Request.Path
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        var option = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true};
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, option));
    }
}