using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.SchemeRegistry;

internal sealed partial class SchemeRegistryClient
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Scheme registry configured: base_uri={BaseUri}")]
    private partial void LogRegistryConfigured(Uri baseUri);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Scheme registry not configured — using default accrual rate")]
    private partial void LogRegistryNotConfigured();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Scheme cache hit: scheme_id={SchemeId} accrual_rate={AccrualRate}")]
    private partial void LogCacheHit(string schemeId, double accrualRate);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Scheme fetch succeeded: scheme_id={SchemeId} accrual_rate={AccrualRate} duration_ms={ElapsedMs:F3}")]
    private partial void LogFetchSucceeded(string schemeId, double accrualRate, double elapsedMs);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Warning,
        Message = "Scheme fetch failed (using default): scheme_id={SchemeId} duration_ms={ElapsedMs:F3}")]
    private partial void LogFetchFailed(Exception exception, string schemeId, double elapsedMs);

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "Scheme resolve summary: requested={Requested} cache_hits={CacheHits} fetched={Fetched}")]
    private partial void LogResolveSummary(int requested, int cacheHits, int fetched);
}
