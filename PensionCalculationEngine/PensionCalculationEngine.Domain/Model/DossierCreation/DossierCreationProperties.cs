using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Domain.Model.DossierCreation;

public sealed class DossierCreationProperties
{
    [JsonPropertyName("dossier_id")]
    public string DossierId { get; set; }

    [JsonPropertyName("person_id")]
    public string PersonId { get; set; } 

    [JsonPropertyName("name")]
    public string Name { get; set; } 

    [JsonPropertyName("birth_date")]
    public DateOnly BirthDate { get; set; }
}