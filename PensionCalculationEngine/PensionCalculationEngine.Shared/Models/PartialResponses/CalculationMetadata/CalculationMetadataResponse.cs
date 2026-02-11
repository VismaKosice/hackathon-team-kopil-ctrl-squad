using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationMetadata;

public class CalculationMetadataResponse
{
    [JsonPropertyName("calculation_id")]
    public Guid CalculationId { get; set; }
    
    [JsonPropertyName("tenant_id")]
    public string TenantId { get; set; } 
    
    [JsonPropertyName("calculation_started_at")]
    public DateTime CalculationStartedAt { get; set; }
    
    [JsonPropertyName("calculation_completed_at")]
    public DateTime CalculationCompletedAt { get; set; }
    
    [JsonPropertyName("calculation_duration_ms")]
    public long CalculationDurationMs { get; set; }
    
    [JsonPropertyName("calculation_outcome")]
    public string CalculationOutcome { get; set; } 
}