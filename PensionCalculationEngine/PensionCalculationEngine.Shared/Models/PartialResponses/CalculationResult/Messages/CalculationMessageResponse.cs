using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Messages;

/// <summary>Validation/calculation message produced during processing.</summary>
public sealed class CalculationMessageResponse
{
    [JsonPropertyName("level")]
    public string Level { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
}
