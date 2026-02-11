using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Domain.Model.ApplyIndexation;

public sealed class ApplyIndexationProperties
{
    [JsonPropertyName("percentage")]
    public decimal Percentage { get; set; }
}