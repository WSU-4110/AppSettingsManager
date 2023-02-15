namespace AppSettingsManagerBff.Model.Api;

public class ApiBaseUser
{
    public string UserId { get; set; }
    public string Password { get; set; }

    public ApiBaseUser(string userId, string password)
    {
        UserId = userId;
        Password = password;
    }
}