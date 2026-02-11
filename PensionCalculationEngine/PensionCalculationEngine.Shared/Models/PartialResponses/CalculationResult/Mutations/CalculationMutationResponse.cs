using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

public class CalculationMutationResponse
{
    [JsonPropertyName("mutation_id")]
    public Guid MutationId { get; set; }
    
    [JsonPropertyName("mutation_definition_name")]
    public string MutationDefinitionName { get; set; } = string.Empty;
    
    [JsonPropertyName("mutation_type")]
    public string MutationType { get; set; } = string.Empty;
    
    [JsonPropertyName("actual_at")]
    public DateOnly ActualAt { get; set; }
    
    [JsonPropertyName("mutation_properties")]
    public Dictionary<string, object> MutationProperties { get; set; } = new();
    
    [JsonPropertyName("dossier_id")]
    public Guid? DossierId { get; set; }
}