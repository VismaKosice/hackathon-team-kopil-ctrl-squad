using System.Text.Json;
using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

public class PolicyReponse
{
    [JsonPropertyName("policy_id")]
    public string PolicyId { get; set; } 

    [JsonPropertyName("scheme_id")]
    public string SchemeId { get; set; } 

    [JsonPropertyName("employment_start_date")]
    public DateOnly EmploymentStartDate { get; set; }

    [JsonPropertyName("salary")]
    public int Salary { get; set; }

    [JsonPropertyName("part_time_factor")]
    public decimal PartTimeFactor { get; set; }

    [JsonPropertyName("attainable_pension")]
    public decimal? AttainablePension { get; set; }

    [JsonPropertyName("projections")]
    public JsonElement? Projections { get; set; }
}