using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.InitialSituation;

/// <summary>Starting state, always { "dossier": null }, with actual_at of the first mutation.</summary>
public sealed class InitialSituationResponse
{
    [JsonPropertyName("actual_at")]
    public DateOnly ActualAt { get; set; }

    [JsonPropertyName("situation")]
    public SituationResponse Situation { get; set; } = new();
}
