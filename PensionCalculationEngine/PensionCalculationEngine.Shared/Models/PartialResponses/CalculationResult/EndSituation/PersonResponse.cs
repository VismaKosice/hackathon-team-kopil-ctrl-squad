using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

/// <summary>Person attached to a dossier (participant).</summary>
public sealed class PersonResponse
{
    [JsonPropertyName("person_id")]
    public Guid Id { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("birth_date")]
    public DateOnly BirthDate { get; set; }
}
