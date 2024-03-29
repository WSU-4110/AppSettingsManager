using System.Text.Json;
using System.Text.Json.Nodes;
using AppSettingsManagerApi.Domain.Conversion;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public class SettingConverter : IBidirectionalConverter<Model.Setting, Setting>
{
    public Setting Convert(Model.Setting source)
    {
        return new Setting
        {
            SettingGroupId = source.SettingGroupId,
            Input = JsonSerializer.Serialize(source.Input.ToJsonString()),
            Version = source.Version,
            IsCurrent = source.IsCurrent,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy
        };
    }

    public Model.Setting Convert(Setting source)
    {
        return new Model.Setting
        {
            SettingGroupId = source.SettingGroupId,
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
