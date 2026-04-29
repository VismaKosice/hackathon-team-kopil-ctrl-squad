using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

/// <summary>Pension policy snapshot in the calculation end situation.</summary>
public sealed class PolicyResponse
{
    [JsonPropertyName("policy_id")]
    public string PolicyId { get; set; } = string.Empty;

    [JsonPropertyName("scheme_id")]
    public string SchemeId { get; set; } = string.Empty;

    [JsonPropertyName("employment_start_date")]
    public DateOnly EmploymentStartDate { get; set; }

    [JsonPropertyName("salary")]
    public decimal Salary { get; set; }

    [JsonPropertyName("part_time_factor")]
    public decimal PartTimeFactor { get; set; }

    [JsonPropertyName("attainable_pension")]
    public decimal? AttainablePension { get; set; }

    [JsonPropertyName("projections")]
    public List<PolicyProjectionResponse>? Projections { get; set; }
}
