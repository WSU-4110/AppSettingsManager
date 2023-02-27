using System.Text.Json;
using System.Text.Json.Nodes;
using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingVersionBiverter : IBidirectionalConverter<Model.SettingVersion, SettingVersion>
{
    public SettingVersion Convert(Model.SettingVersion source)
    {
        return new SettingVersion
        {
            SettingGroupId = source.Id,
            Input = JsonSerializer.Serialize(source.Input.ToJsonString()),
            Version = source.Version,
            IsCurrent = source.IsCurrent,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy
        };
    }

    public Model.SettingVersion Convert(SettingVersion source)
    {
        return new Model.SettingVersion
        {
            Id = source.SettingGroupId,
            // Can deserialize a string to Json object like this
            // The '!' tells .net to trust that this won't be null, be careful using this
            Input = JsonSerializer.Deserialize<JsonNode>(source.Input)!,
            Version = source.Version,
            IsCurrent = source.IsCurrent,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy
        };
    }
}
