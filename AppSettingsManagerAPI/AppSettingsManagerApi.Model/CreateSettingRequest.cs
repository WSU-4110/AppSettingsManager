using System.Text.Json.Nodes;

namespace AppSettingsManagerApi.Model;

public class CreateSettingRequest
{
    public string Id { get; set; }
    public JsonNode Input { get; set; }
    public string CreatedBy { get; set; }
}
