using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Domain.Model.Dossier;

public sealed class DossierProperties
{
    [JsonPropertyName("scheme_id")]
    public string SchemeId { get; set; }

    [JsonPropertyName("employment_start_date")]
    public DateOnly EmploymentStartDate { get; set; }

    [JsonPropertyName("salary")]
    public decimal Salary { get; set; }

    [JsonPropertyName("part_time_factor")]
    public decimal PartTimeFactor { get; set; }
}