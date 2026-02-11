using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialRequests;

public class MutationBaseRequest
{
    [JsonPropertyName("mutation_id")]
    public Guid MutationId { get; set; }
    
    [JsonPropertyName("dossier_id")]
    public Guid? DossierId { get; set; }
    
    [JsonPropertyName("mutation_type")]
    public string MutationType { get; set; }

    [JsonPropertyName("mutation_definition_name")]
    public string MutationDefinitionName { get; set; }

    [JsonPropertyName("actual_at")]
    public DateOnly ActualAt { get; set; }
    
    [JsonPropertyName("mutation_properties")]
    public Dictionary<string, object> MutationProperties { get; set; }
}