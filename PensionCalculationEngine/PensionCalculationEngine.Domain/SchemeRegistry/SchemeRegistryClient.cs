using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Json;

namespace PensionCalculationEngine.Domain.SchemeRegistry;

/// <summary>
/// HTTP-backed scheme registry client. Caches accrual rates by scheme_id across requests,
/// fetches unknown schemes in parallel, falls back to <see cref="Constants.Calculation.DefaultAccrualRate"/>
/// on any error or when SCHEME_REGISTRY_URL is unset.
/// </summary>
internal sealed partial class SchemeRegistryClient : ISchemeRegistryClient
{
    private static readonly TimeSpan PerCallTimeout = TimeSpan.FromSeconds(2);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SchemeRegistryClient> _logger;
    private readonly Uri? _baseUri;
    private readonly ConcurrentDictionary<string, double> _cache = new(StringComparer.Ordinal);

    public SchemeRegistryClient(
        IHttpClientFactory httpClientFactory,
        ILogger<SchemeRegistryClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        var url = Environment.GetEnvironmentVariable("SCHEME_REGISTRY_URL");
        if (!string.IsNullOrWhiteSpace(url) && Uri.TryCreate(EnsureTrailingSlash(url), UriKind.Absolute, out var uri))
        {
            _baseUri = uri;
            LogRegistryConfigured(uri);
        }
        else
        {
            LogRegistryNotConfigured();
        }
    }

    public async ValueTask<IReadOnlyDictionary<string, double>> GetAccrualRatesAsync(
        IReadOnlyCollection<string> schemeIds,
        CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, double>(schemeIds.Count, StringComparer.Ordinal);

        if (_baseUri is null)
        {
            foreach (var id in schemeIds)
            {
                result[id] = Constants.Calculation.DefaultAccrualRate;
            }

            return result;
        }

        List<Task<KeyValuePair<string, double>>>? pending = null;
        var cacheHits = 0;

        foreach (var schemeId in schemeIds)
        {
            if (result.ContainsKey(schemeId))
            {
                continue;
            }

            if (_cache.TryGetValue(schemeId, out var cached))
            {
                result[schemeId] = cached;
                cacheHits++;
                LogCacheHit(schemeId, cached);
                continue;
            }

            pending ??= new List<Task<KeyValuePair<string, double>>>();
            pending.Add(FetchAsync(schemeId, cancellationToken));
        }

        if (pending is not null)
        {
            var fetched = await Task.WhenAll(pending).ConfigureAwait(false);
            for (var i = 0; i < fetched.Length; i++)
            {
                var kvp = fetched[i];
                _cache[kvp.Key] = kvp.Value;
                result[kvp.Key] = kvp.Value;
            }
        }

        LogResolveSummary(schemeIds.Count, cacheHits, pending?.Count ?? 0);
        return result;
    }

    private async Task<KeyValuePair<string, double>> FetchAsync(string schemeId, CancellationToken cancellationToken)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(PerCallTimeout);

            var client = _httpClientFactory.CreateClient(nameof(SchemeRegistryClient));
            var requestUri = new Uri(_baseUri!, $"schemes/{Uri.EscapeDataString(schemeId)}");

            var response = await client
                .GetFromJsonAsync(requestUri, PensionJsonContext.Default.SchemePayload, cts.Token)
                .ConfigureAwait(false);

            var rate = response?.AccrualRate ?? Constants.Calculation.DefaultAccrualRate;
            var elapsedMs = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;

            LogFetchSucceeded(schemeId, rate, elapsedMs);
            return new KeyValuePair<string, double>(schemeId, rate);
        }
        catch (Exception ex)
        {
            var elapsedMs = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
            LogFetchFailed(ex, schemeId, elapsedMs);
            return new KeyValuePair<string, double>(schemeId, Constants.Calculation.DefaultAccrualRate);
        }
    }

    private static string EnsureTrailingSlash(string url)
        => url.EndsWith('/')
            ? url
            : url + "/";
}
