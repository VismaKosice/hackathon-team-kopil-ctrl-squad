using System.Text.Json;

namespace PensionCalculationEngine.Domain.Mutations.Properties;

/// <summary>Strongly-typed view over <c>project_future_benefits</c> mutation_properties.</summary>
internal readonly struct ProjectFutureBenefitsProperties
{
    public required DateOnly ProjectionStartDate { get; init; }
    public required DateOnly ProjectionEndDate { get; init; }
    public required int ProjectionIntervalMonths { get; init; }

    public static ProjectFutureBenefitsProperties From(in JsonElement source)
    {
        var start = source.TryGetProperty("projection_start_date", out var s)
                    && s.ValueKind == JsonValueKind.String
                    && DateOnly.TryParse(s.GetString(), out var parsedStart)
            ? parsedStart
            : default;

        var end = source.TryGetProperty("projection_end_date", out var e)
                  && e.ValueKind == JsonValueKind.String
                  && DateOnly.TryParse(e.GetString(), out var parsedEnd)
            ? parsedEnd
            : default;

        var interval = source.TryGetProperty("projection_interval_months", out var i)
                       && i.ValueKind == JsonValueKind.Number
            ? i.GetInt32()
            : 0;

        return new ProjectFutureBenefitsProperties
        {
            ProjectionStartDate = start,
            ProjectionEndDate = end,
            ProjectionIntervalMonths = interval,
        };
    }
}
