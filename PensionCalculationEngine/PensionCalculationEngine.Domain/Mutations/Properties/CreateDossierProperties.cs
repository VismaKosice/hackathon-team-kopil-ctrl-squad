using System.Text.Json;

namespace PensionCalculationEngine.Domain.Mutations.Properties;

/// <summary>Strongly-typed view over <c>create_dossier</c> mutation_properties.</summary>
internal readonly struct CreateDossierProperties
{
    public required Guid DossierId { get; init; }
    public required Guid PersonId { get; init; }
    public required string? Name { get; init; }
    public required DateOnly? BirthDate { get; init; }
    public required bool BirthDateValid { get; init; }

    public static CreateDossierProperties From(in JsonElement source)
    {
        var dossierId = source.TryGetProperty("dossier_id", out var d) && d.ValueKind == JsonValueKind.String
            ? d.GetGuid()
            : Guid.Empty;

        var personId = source.TryGetProperty("person_id", out var p) && p.ValueKind == JsonValueKind.String
            ? p.GetGuid()
            : Guid.Empty;

        var name = source.TryGetProperty("name", out var n) && n.ValueKind == JsonValueKind.String
            ? n.GetString()
            : null;

        DateOnly? birthDate = null;
        var birthDateValid = false;
        if (source.TryGetProperty("birth_date", out var b) && b.ValueKind == JsonValueKind.String
            && DateOnly.TryParse(b.GetString(), out var parsed))
        {
            birthDate = parsed;
            birthDateValid = true;
        }

        return new CreateDossierProperties
        {
            DossierId = dossierId,
            PersonId = personId,
            Name = name,
            BirthDate = birthDate,
            BirthDateValid = birthDateValid,
        };
    }
}
