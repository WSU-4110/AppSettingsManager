namespace AppSettingsManagerApi.Model;

/// <summary>
/// This is the User model that will actually get returned by the API
///
/// There are limitations on what data types you can store in a database,
/// so you'll need to convert things to more usable forms
///
/// The repository interfaces also cannot reference the models in the 'Infrastructure' project because
/// that would make the 'Domain' project dependent on 'Infrastructure' which would be a circular dependency,
/// since the 'Infrastructure' project is already dependent on 'Domain'
/// </summary>
public class User
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
