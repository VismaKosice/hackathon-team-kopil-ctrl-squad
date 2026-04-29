using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

/// <summary>Situation snapshot — wraps the dossier (null before create_dossier).</summary>
public sealed class SituationResponse
{
    [JsonPropertyName("dossier")]
    public DossierResponse? Dossier { get; set; }
}
