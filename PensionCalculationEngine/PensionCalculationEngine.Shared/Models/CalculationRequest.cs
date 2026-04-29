using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Shared.Models;

/// <summary>Root request body for POST /calculation-requests.</summary>
public sealed class CalculationRequest
{
    [JsonPropertyName("tenant_id")]
    public string TenantId { get; set; } = string.Empty;

    [JsonPropertyName("calculation_instructions")]
    public CalculationInstructionsRequest CalculationInstructions { get; set; } = new();
}
