namespace AppSettingsManagerBff.Model.Requests;

public class CreateUserRequest
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}