using System.Text.Json.Nodes;

namespace AppSettingsManagerBff.Model.Api;

// Should be pretty much the same as what is returned by API controller method that is being called
public class ApiSetting
{
    public string Id { get; set; }
    public JsonNode Input { get; set; }
    public int Version { get; set; }
    public bool IsCurrent { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    // Need this constructor so that this object can be created on deserialization
    public ApiSetting(
        string id,
        JsonNode input,
        int version,
        bool isCurrent,
        string createdBy,
        DateTimeOffset createdAt
    )
    {
        Id = id;
        Input = input;
        Version = version;
        IsCurrent = isCurrent;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }
}
