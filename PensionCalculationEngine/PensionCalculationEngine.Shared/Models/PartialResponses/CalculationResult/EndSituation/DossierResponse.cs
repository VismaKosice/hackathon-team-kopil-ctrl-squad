using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

public class DossierResponse
{
    [JsonPropertyName("dossier_id")]
    public string Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("retirement_date")]
    public DateOnly? RetirementDate { get; set; }

    [JsonPropertyName("persons")]
    public List<PersonResponse> Persons { get; set; }

    [JsonPropertyName("policies")]
    public List<PolicyReponse> Policies { get; set; }
}