using System.Text.Json.Serialization;
using PensionCalculationEngine.Domain.SchemeRegistry;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Domain.Json;

/// <summary>
/// Source-generated JsonSerializerContext covering every (de)serialization root used by the
/// engine. Required for AOT publish — eliminates reflection-based metadata gathering.
/// Transitive types (DTOs nested under these roots) are picked up automatically by the generator.
/// </summary>
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    PropertyNameCaseInsensitive = false,
    GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(CalculationRequest))]
[JsonSerializable(typeof(CalculationResponse))]
[JsonSerializable(typeof(SituationResponse))]
[JsonSerializable(typeof(SchemePayload))]
internal partial class PensionJsonContext : JsonSerializerContext
{
}
