using System.Text.Json.Serialization.Metadata;

namespace PensionCalculationEngine.Domain.Json;

/// <summary>
/// Public accessor for the source-generated <see cref="PensionJsonContext"/>.
/// The context itself is internal (it references internal types), but exposing the
/// <see cref="IJsonTypeInfoResolver"/> lets the API layer register it without taking
/// an ASP.NET dependency on the Domain project.
/// </summary>
public static class PensionJsonContextProvider
{
    public static IJsonTypeInfoResolver Default => PensionJsonContext.Default;
}
