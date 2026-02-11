using System.Text.Json;

namespace PensionCalculationEngine.Domain.Extensions;

public static class JsonValueHelper
{
    public static string GetStringValue(this Dictionary<string, object> source, string propertyName)
    {
        return GetValueOrThrowError(source, propertyName, JsonValueKind.String).ToString();
    } 
    public static Guid GetGuidValue(this Dictionary<string, object> source, string propertyName)
    {
        var value = GetValueOrThrowError(source, propertyName, JsonValueKind.String);
        try
        {
            return value.GetGuid();
        }
        catch
        {
            return new Guid();
        }

        //return GetValueOrThrowError(source, propertyName, JsonValueKind.String).GetGuid();
    } 
    public static int GetIntValue(this Dictionary<string, object> source, string propertyName)
    {
        return GetValueOrThrowError(source, propertyName, JsonValueKind.Number).GetInt32();
    } 
    public static double GetDoubleValue(this Dictionary<string, object> source, string propertyName)
    {
        return GetValueOrThrowError(source, propertyName, JsonValueKind.Number).GetDouble();
    } 
    public static decimal GetDecimalValue(this Dictionary<string, object> source, string propertyName)
    {
        return GetValueOrThrowError(source, propertyName, JsonValueKind.Number).GetDecimal();
    } 
    public static DateOnly GetDateOnlyValue(this Dictionary<string, object> source, string propertyName)
    {
        return DateOnly.Parse(GetValueOrThrowError(source, propertyName, JsonValueKind.String).ToString());
    } 
    

    private static JsonElement GetValueOrThrowError(Dictionary<string, object> source, string key, JsonValueKind valueKind)
    {
        if (source.TryGetValue(key, out var value)
            && value is JsonElement element
            && element.ValueKind == valueKind)
        {
            return element;
        }

        throw new ArgumentException($"Key {key} not found");
    }
}