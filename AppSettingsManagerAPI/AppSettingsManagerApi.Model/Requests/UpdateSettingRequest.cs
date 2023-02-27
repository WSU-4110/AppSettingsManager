using System.Text.Json.Nodes;

namespace AppSettingsManagerApi.Model.Requests;

public class UpdateSettingRequest
{
    public string Id { get; set; }
    public JsonNode Input { get; set; }
    public string CreatedBy { get; set; }
}
