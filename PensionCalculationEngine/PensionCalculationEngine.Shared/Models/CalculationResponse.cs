using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationMetadata;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult;

namespace PensionCalculationEngine.Shared.Models;

/// <summary>Root response for POST /calculation-requests.</summary>
public sealed class CalculationResponse
{
    [JsonPropertyName("calculation_metadata")]
    public CalculationMetadataResponse CalculationMetadata { get; set; } = new();

    [JsonPropertyName("calculation_result")]
    public CalculationResultResponse CalculationResult { get; set; } = new();
}
