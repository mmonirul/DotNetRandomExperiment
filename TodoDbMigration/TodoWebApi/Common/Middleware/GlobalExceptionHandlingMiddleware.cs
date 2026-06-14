using System.Net;
using System.Text.Json;
using TodoWebApi.Common.Exceptions;

namespace TodoWebApi.Common.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ApiResponse();

        switch (exception)
        {
            case ApiException apiException:
                context.Response.StatusCode = apiException.StatusCode;
                response.Status = apiException.StatusCode;
                response.Message = apiException.Message;
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Status = (int)HttpStatusCode.NotFound;
                response.Message = "The requested resource was not found.";
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = _env.IsDevelopment() 
                    ? exception.Message 
                    : "An internal server error occurred.";
                break;
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
