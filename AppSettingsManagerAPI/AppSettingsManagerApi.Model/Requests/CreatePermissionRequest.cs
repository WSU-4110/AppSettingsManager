namespace AppSettingsManagerApi.Model.Requests;

public class CreatePermissionRequest
{
    public string UserId { get; set; }
    public string SettingGroupId { get; set; }
    public PermissionLevel StartingPermissionLevel { get; set; }
    private bool NeedsApproval = true;

    public bool GetNeedsApproval()
    {
        return NeedsApproval;
    }

    public void SetNeedsApproval(bool needsApproval)
    {
        NeedsApproval = needsApproval;
    }
}
