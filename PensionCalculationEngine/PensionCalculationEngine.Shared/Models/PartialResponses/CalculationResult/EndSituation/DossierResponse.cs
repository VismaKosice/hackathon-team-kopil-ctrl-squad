using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

/// <summary>Dossier (participant pension file) in the calculation end situation.</summary>
public sealed class DossierResponse
{
    [JsonPropertyName("dossier_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("retirement_date")]
    public DateOnly? RetirementDate { get; set; }

    [JsonPropertyName("persons")]
    public List<PersonResponse> Persons { get; set; } = [];

    [JsonPropertyName("policies")]
    public List<PolicyResponse> Policies { get; set; } = [];
}
