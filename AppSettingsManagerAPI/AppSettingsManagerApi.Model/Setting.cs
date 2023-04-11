using System.Text.Json.Nodes;

namespace AppSettingsManagerApi.Model;

public class Setting
{
    public string SettingGroupId { get; set; }

    // A JsonNode allows you to interact with a Json object with additional operations
    // One big advantage of JsonNode is the ability to reference values like a dictionary i.e. var x = JsonNode[key]
    public JsonNode Input { get; set; }
    public int Version { get; set; }
    public bool IsCurrent { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
