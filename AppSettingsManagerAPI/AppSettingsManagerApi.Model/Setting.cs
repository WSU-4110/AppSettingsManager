using System.Text.Json.Nodes;

namespace AppSettingsManagerApi.Model;

public class Setting
{
    public string Id { get; set; }
    public JsonNode Input { get; set; }
    public int Version { get; set; }
    public bool IsCurrent { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}