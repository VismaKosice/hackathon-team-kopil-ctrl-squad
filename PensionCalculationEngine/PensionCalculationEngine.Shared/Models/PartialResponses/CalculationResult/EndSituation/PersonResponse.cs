using System.Text.Json.Serialization;

namespace PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

public class PersonResponse
{
    [JsonPropertyName("person_id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("Role")]
    public string Role { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("birth_date")]
    public DateOnly BirthDate { get; set; }
}