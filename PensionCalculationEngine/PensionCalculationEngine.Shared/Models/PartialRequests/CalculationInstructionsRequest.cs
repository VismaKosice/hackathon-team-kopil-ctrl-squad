using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialRequests;

/// <summary>Wrapper for the ordered list of mutations to apply.</summary>
public sealed class CalculationInstructionsRequest
{
    [JsonPropertyName("mutations")]
    public List<MutationBaseRequest> Mutations { get; set; } = [];
}
