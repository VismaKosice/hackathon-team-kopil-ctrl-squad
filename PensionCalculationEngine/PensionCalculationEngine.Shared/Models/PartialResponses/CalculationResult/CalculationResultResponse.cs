using System.Text.Json.Serialization;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.InitialSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Messages;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult;

/// <summary>The calculation_result block of the response.</summary>
public sealed class CalculationResultResponse
{
    [JsonPropertyName("messages")]
    public List<CalculationMessageResponse> Messages { get; set; } = [];

    [JsonPropertyName("initial_situation")]
    public InitialSituationResponse InitialSituation { get; set; } = new();

    [JsonPropertyName("mutations")]
    public List<MutationDoneResponse> Mutations { get; set; } = [];

    [JsonPropertyName("end_situation")]
    public EndSituationResponse EndSituation { get; set; } = new();
}
