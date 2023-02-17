namespace AppSettingsManagerBff.Model.Api;

// Should be pretty much the same as what is returned by API controller method that is being called
public class ApiBaseUser
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public IEnumerable<ApiSetting> Settings { get; set; }

    // Need this constructor so that this object can be created on deserialization
    public ApiBaseUser(string userId, string password, IEnumerable<ApiSetting> settings)
    {
        UserId = userId;
        Password = password;
        Settings = settings;
    }
}
