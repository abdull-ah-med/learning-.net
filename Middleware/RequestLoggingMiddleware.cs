using System.Diagnostics;

namespace AuthApp.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(context);
        stopwatch.Stop();
        var duration = stopwatch.ElapsedMilliseconds;
        
        _logger.LogInformation("[{Method}] {Path} completed in {Duration}ms with status {StatusCode}", 
            context.Request.Method, 
            context.Request.Path, 
            duration, 
            context.Response.StatusCode);
    }
}
