using System.Text.Json.Nodes;

namespace AppSettingsManagerBff.Model.Api;

public class UpdateSettingRequest
{
    public string Id { get; set; }
    public JsonNode Input { get; set; }
    public string CreatedBy { get; set; }
}
