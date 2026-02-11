using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationMetadata;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult;

namespace PensionCalculationEngine.Shared.Models;

public class CalculationResponse
{
    [JsonPropertyName("calculation_metadata")]
    public CalculationMetadataResponse CalculationMetadata { get; set; } 
    
    [JsonPropertyName("calculation_result")]
    public CalculationResultResponse CalculationResult { get; set; }
}