using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult;

public class CalculationResultResponse
{
    [JsonPropertyName("messages")]
    public List<string> Messages { get; set; }
    
    [JsonPropertyName("mutations")]
    public List<MutationDoneResponse> Mutations { get; set; } = new();
    
    [JsonPropertyName("end_situation")]
    public EndSituationReponse EndSituation { get; set; }
}