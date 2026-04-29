using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Domain.SchemeRegistry;

/// <summary>Wire shape returned by the external Scheme Registry service.</summary>
internal sealed class SchemePayload
{
    [JsonPropertyName("scheme_id")]
    public string? SchemeId { get; set; }

    [JsonPropertyName("accrual_rate")]
    public double? AccrualRate { get; set; }
}
