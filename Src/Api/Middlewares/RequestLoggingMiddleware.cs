namespace Api.Middlewares;

public class RequestLoggingMiddleware : IMiddleware
{
    readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var ip = context.Connection.RemoteIpAddress?.ToString();

        _logger.LogInformation("{Method} {Path} called from {IP} at {Time}", method, path, ip, DateTime.UtcNow);

        await next(context);
    }
}