using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

/// <summary>Single projection point for the project_future_benefits bonus mutation.</summary>
public sealed class PolicyProjectionResponse
{
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("projected_pension")]
    public decimal ProjectedPension { get; set; }
}
