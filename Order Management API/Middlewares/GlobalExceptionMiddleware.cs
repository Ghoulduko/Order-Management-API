using Order.Core.Exceptions;

namespace Order_Management_API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            NotFoundException => 404,
            UnauthorizedAccessException => 401,
            ArgumentException => 400,
            _ => 500
        };
        
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(new {error = ex.Message});
    }
}