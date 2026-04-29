using System.Diagnostics;

namespace PensionCalculationEngine.Api.Middleware;

/// <summary>
/// Measures wall-clock execution time of every HTTP request and emits a Debug-level
/// log entry with method, path, status code, and elapsed milliseconds.
/// </summary>
internal sealed partial class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(
        RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        finally
        {
            var elapsedMs = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
            LogRequestCompleted(
                context.Request.Method,
                context.Request.Path.Value ?? string.Empty,
                context.Response.StatusCode,
                elapsedMs);
        }
    }
}
