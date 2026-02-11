using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

public class MutationDoneResponse
{
    [JsonPropertyName("mutation")]
    public CalculationMutationResponse Mutation { get; set; } = new();
    
    [JsonPropertyName("calculation_message_indexes")]
    public List<int> CalculationMessageIndexes { get; set; } = new();
    
//    [JsonPropertyName("forward_patch_to_situation_after_this_mutation")]
//    public List<JsonPatchOperation>? ForwardPatchToSituationAfterThisMutation { get; set; }
//    
//    [JsonPropertyName("backward_patch_to_previous_situation")]
//    public List<JsonPatchOperation>? BackwardPatchToPreviousSituation { get; set; }
}