using System.Text.Json;
using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialRequests;

/// <summary>Single mutation entry inside the request's calculation_instructions.mutations array.</summary>
public sealed class MutationBaseRequest
{
    [JsonPropertyName("mutation_id")]
    public Guid MutationId { get; set; }

    [JsonPropertyName("dossier_id")]
    public Guid? DossierId { get; set; }

    [JsonPropertyName("mutation_type")]
    public string MutationType { get; set; } = string.Empty;

    [JsonPropertyName("mutation_definition_name")]
    public string MutationDefinitionName { get; set; } = string.Empty;

    [JsonPropertyName("actual_at")]
    public DateOnly ActualAt { get; set; }

    [JsonPropertyName("mutation_properties")]
    public JsonElement MutationProperties { get; set; }
}
