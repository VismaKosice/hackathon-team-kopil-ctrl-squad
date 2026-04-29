using System.Text.Json;

namespace PensionCalculationEngine.Domain.Mutations.Properties;

/// <summary>Strongly-typed view over <c>apply_indexation</c> mutation_properties.</summary>
internal readonly struct ApplyIndexationProperties
{
    public required decimal Percentage { get; init; }
    public required string? SchemeId { get; init; }
    public required DateOnly? EffectiveBefore { get; init; }

    public static ApplyIndexationProperties From(in JsonElement source)
    {
        var percentage = source.TryGetProperty("percentage", out var p) && p.ValueKind == JsonValueKind.Number
            ? p.GetDecimal()
            : 0m;

        var schemeId = source.TryGetProperty("scheme_id", out var s) && s.ValueKind == JsonValueKind.String
            ? s.GetString()
            : null;

        DateOnly? effectiveBefore = null;
        if (source.TryGetProperty("effective_before", out var e) && e.ValueKind == JsonValueKind.String
            && DateOnly.TryParse(e.GetString(), out var parsed))
        {
            effectiveBefore = parsed;
        }

        return new ApplyIndexationProperties
        {
            Percentage = percentage,
            SchemeId = schemeId,
            EffectiveBefore = effectiveBefore,
        };
    }
}
