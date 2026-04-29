namespace PensionCalculationEngine.Domain.SchemeRegistry;

/// <summary>
/// Resolves accrual rates for the given scheme ids — from the external Scheme Registry when
/// SCHEME_REGISTRY_URL is configured, falling back to the default rate on missing config,
/// timeouts, or any error.
/// </summary>
public interface ISchemeRegistryClient
{
    ValueTask<IReadOnlyDictionary<string, double>> GetAccrualRatesAsync(
        IReadOnlyCollection<string> schemeIds,
        CancellationToken cancellationToken = default);
}
