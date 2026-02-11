using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

public class EndSituationReponse
{
    [JsonPropertyName("mutation_id")]
    public string MutationId { get; set; }
    
    [JsonPropertyName("mutation_index")]
    public int MutationIndex { get; set; }
    
    [JsonPropertyName("actual_at")]
    public DateOnly? ActualAt { get; set; }
    
    [JsonPropertyName("situation")]
    public SituationReponse Situation { get; set; }
}