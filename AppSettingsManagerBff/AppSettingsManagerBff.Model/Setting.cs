using System;
using System.Text.Json.Nodes;

namespace AppSettingsManagerBff.Model;

public class Setting
{
    public string SettingGroupId { get; set; }
    public JsonNode Input { get; set; }
    public int Version { get; set; }
    public bool IsCurrent { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}