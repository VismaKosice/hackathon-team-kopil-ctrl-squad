namespace PensionCalculationEngine.Api.Middleware;

internal sealed partial class RequestTimingMiddleware
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "HTTP {Method} {Path} -> {StatusCode} in {ElapsedMs:F3} ms")]
    private partial void LogRequestCompleted(
        string method,
        string path,
        int statusCode,
        double elapsedMs);
}
