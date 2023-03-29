using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSettingsManagerApi.Infrastructure.MySql;

/// <summary>
/// This class contains the settings/variables stored in the database and relevant metadata
/// </summary>
public class Setting
{
    [MaxLength(36)]
    [ForeignKey("SettingGroup")]
    public string SettingGroupId { get; set; }
    public SettingGroup SettingGroup { get; set; }
    public string Input { get; set; }
    public int Version { get; set; }

    // Will indicate whether this is the version of variables being used by your application
    public bool IsCurrent { get; set; }

    [MaxLength(36)]
    public string CreatedBy { get; set; }

    // This timestamp label should make this variable automatically update whenever a record is updated in EF
    [Timestamp]
    public byte[] LastUpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
