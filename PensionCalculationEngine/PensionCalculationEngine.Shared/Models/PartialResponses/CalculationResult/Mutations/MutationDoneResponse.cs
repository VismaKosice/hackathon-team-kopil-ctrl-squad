using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

/// <summary>Wrapper attaching message indexes and JSON Patches (forward + backward) to a processed mutation.</summary>
public sealed class MutationDoneResponse
{
    [JsonPropertyName("mutation")]
    public CalculationMutationResponse Mutation { get; set; } = new();

    [JsonPropertyName("calculation_message_indexes")]
    public List<int> CalculationMessageIndexes { get; set; } = [];

    [JsonPropertyName("forward_patch_to_situation_after_this_mutation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JsonNode>? ForwardPatchToSituationAfterThisMutation { get; set; }

    [JsonPropertyName("backward_patch_to_previous_situation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JsonNode>? BackwardPatchToPreviousSituation { get; set; }
}
