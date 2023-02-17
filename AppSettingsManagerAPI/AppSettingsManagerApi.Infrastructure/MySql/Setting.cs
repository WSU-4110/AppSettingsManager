using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace AppSettingsManagerApi.Infrastructure.MySql;

/// <summary>
/// This class contains the settings/variables stored in the database and relevant metadata
/// </summary>
public class Setting
{
    [MaxLength(36)]
    public string Id { get; set; }

    // A JsonNode allows you to interact with a Json object with additional operations
    // One big advantage of JsonNode is the ability to reference values like a dictionary i.e. var x = JsonNode[key]
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
    // Commenting these out until we implement permissions
    /*public bool IsApproved { get; set; }
    public DateTimeOffset ApprovedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public List<Permission> Permissions { get; set; }*/
}
