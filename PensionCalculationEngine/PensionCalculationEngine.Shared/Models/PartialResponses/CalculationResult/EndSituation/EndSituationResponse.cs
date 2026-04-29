using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

/// <summary>End-state of the calculation, pointing at the last successfully applied mutation.</summary>
public sealed class EndSituationResponse
{
    [JsonPropertyName("mutation_id")]
    public Guid MutationId { get; set; }

    [JsonPropertyName("mutation_index")]
    public int MutationIndex { get; set; }

    [JsonPropertyName("actual_at")]
    public DateOnly ActualAt { get; set; }

    [JsonPropertyName("situation")]
    public SituationResponse Situation { get; set; } = new();
}
