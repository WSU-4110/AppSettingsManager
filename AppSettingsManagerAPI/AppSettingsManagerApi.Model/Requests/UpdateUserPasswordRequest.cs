namespace AppSettingsManagerApi.Model.Requests;

public class UpdateUserPasswordRequest
{
    public string UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
