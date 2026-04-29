using System.Text.Json.Nodes;

namespace PensionCalculationEngine.Domain.JsonPatch;

/// <summary>
/// Produces an RFC 6902 JSON Patch (as a list of JsonObjects) by diffing two JsonNode trees.
/// Each operation is a JsonObject with at minimum <c>op</c> and <c>path</c>; <c>value</c> is set
/// for add/replace operations.
/// </summary>
internal static class JsonPatchDiff
{
    private const string OpAdd = "add";
    private const string OpRemove = "remove";
    private const string OpReplace = "replace";

    public static List<JsonNode> Diff(JsonNode? before, JsonNode? after)
    {
        var ops = new List<JsonNode>();
        DiffRecursive("", before, after, ops);
        return ops;
    }

    private static void DiffRecursive(
        string path,
        JsonNode? before,
        JsonNode? after,
        List<JsonNode> ops)
    {
        if (NodesEqual(before, after))
        {
            return;
        }

        if (before is JsonObject beforeObject && after is JsonObject afterObject)
        {
            DiffObjects(path, beforeObject, afterObject, ops);
            return;
        }

        if (before is JsonArray beforeArray && after is JsonArray afterArray)
        {
            DiffArrays(path, beforeArray, afterArray, ops);
            return;
        }

        ops.Add(MakeOp(OpReplace, path, after));
    }

    private static void DiffObjects(
        string path,
        JsonObject before,
        JsonObject after,
        List<JsonNode> ops)
    {
        foreach (var kvp in before)
        {
            var key = kvp.Key;
            var childPath = $"{path}/{EscapePath(key)}";

            if (!after.ContainsKey(key))
            {
                ops.Add(MakeOp(OpRemove, childPath, value: null, includeValue: false));
                continue;
            }

            DiffRecursive(childPath, kvp.Value, after[key], ops);
        }

        foreach (var kvp in after)
        {
            if (before.ContainsKey(kvp.Key))
            {
                continue;
            }

            var childPath = $"{path}/{EscapePath(kvp.Key)}";
            ops.Add(MakeOp(OpAdd, childPath, kvp.Value));
        }
    }

    private static void DiffArrays(
        string path,
        JsonArray before,
        JsonArray after,
        List<JsonNode> ops)
    {
        var beforeCount = before.Count;
        var afterCount = after.Count;
        var minCount = Math.Min(beforeCount, afterCount);

        for (var i = 0; i < minCount; i++)
        {
            DiffRecursive($"{path}/{i}", before[i], after[i], ops);
        }

        for (var i = minCount; i < afterCount; i++)
        {
            ops.Add(MakeOp(OpAdd, $"{path}/-", after[i]));
        }

        for (var i = beforeCount - 1; i >= afterCount; i--)
        {
            ops.Add(MakeOp(OpRemove, $"{path}/{i}", value: null, includeValue: false));
        }
    }

    private static JsonObject MakeOp(string op, string path, JsonNode? value, bool includeValue = true)
    {
        var result = new JsonObject
        {
            ["op"] = op,
            ["path"] = path,
        };

        if (includeValue)
        {
            result["value"] = value?.DeepClone();
        }

        return result;
    }

    private static bool NodesEqual(JsonNode? a, JsonNode? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return JsonNode.DeepEquals(a, b);
    }

    private static string EscapePath(string segment)
    {
        if (segment.Length == 0)
        {
            return segment;
        }

        if (!segment.Contains('~') && !segment.Contains('/'))
        {
            return segment;
        }

        return segment.Replace("~", "~0").Replace("/", "~1");
    }
}
