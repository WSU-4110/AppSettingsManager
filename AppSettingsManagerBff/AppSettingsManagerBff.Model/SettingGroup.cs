using System.Collections.Generic;
using System;

namespace AppSettingsManagerBff.Model;

public class SettingGroup
{
    public string Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public IEnumerable<Setting> Settings { get; set; }
    public IEnumerable<Permission> Permissions { get; set; }
}