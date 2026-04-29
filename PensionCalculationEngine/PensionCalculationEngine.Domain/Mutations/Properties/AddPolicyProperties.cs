using System.Text.Json;

namespace PensionCalculationEngine.Domain.Mutations.Properties;

/// <summary>Strongly-typed view over <c>add_policy</c> mutation_properties.</summary>
internal readonly struct AddPolicyProperties
{
    public required string SchemeId { get; init; }
    public required DateOnly EmploymentStartDate { get; init; }
    public required decimal Salary { get; init; }
    public required decimal PartTimeFactor { get; init; }

    public static AddPolicyProperties From(in JsonElement source)
    {
        var schemeId = source.TryGetProperty("scheme_id", out var s) && s.ValueKind == JsonValueKind.String
            ? s.GetString() ?? string.Empty
            : string.Empty;

        var employmentStartDate = source.TryGetProperty("employment_start_date", out var d)
                                  && d.ValueKind == JsonValueKind.String
                                  && DateOnly.TryParse(d.GetString(), out var parsed)
            ? parsed
            : default;

        var salary = source.TryGetProperty("salary", out var sa) && sa.ValueKind == JsonValueKind.Number
            ? sa.GetDecimal()
            : 0m;

        var partTimeFactor = source.TryGetProperty("part_time_factor", out var pt)
                             && pt.ValueKind == JsonValueKind.Number
            ? pt.GetDecimal()
            : 0m;

        return new AddPolicyProperties
        {
            SchemeId = schemeId,
            EmploymentStartDate = employmentStartDate,
            Salary = salary,
            PartTimeFactor = partTimeFactor,
        };
    }
}
