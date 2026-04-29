using System.Text.Json;

namespace PensionCalculationEngine.Domain.Mutations.Properties;

/// <summary>Strongly-typed view over <c>calculate_retirement_benefit</c> mutation_properties.</summary>
internal readonly struct CalculateRetirementBenefitProperties
{
    public required DateOnly RetirementDate { get; init; }

    public static CalculateRetirementBenefitProperties From(in JsonElement source)
    {
        var retirementDate = source.TryGetProperty("retirement_date", out var r)
                             && r.ValueKind == JsonValueKind.String
                             && DateOnly.TryParse(r.GetString(), out var parsed)
            ? parsed
            : default;

        return new CalculateRetirementBenefitProperties
        {
            RetirementDate = retirementDate,
        };
    }
}
