using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialRequests;

public sealed class CalculationInstructionsRequest
{
    [JsonPropertyName("mutations")]
    public List<MutationBaseRequest> Mutations { get; set; }
}