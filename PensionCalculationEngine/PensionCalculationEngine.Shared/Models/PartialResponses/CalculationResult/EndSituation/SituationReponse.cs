using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

public class SituationReponse
{
    [JsonPropertyName("dossier")]
    public DossierResponse Dossier { get; set; }
}