using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Shared.Models;

public sealed class CalculationRequest
{
    [JsonPropertyName("tenant_id")]
    public string TenantId { get; set; }

    [JsonPropertyName("calculation_instructions")]
    public CalculationInstructionsRequest CalculationInstructions { get; set; }
}