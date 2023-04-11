using System.ComponentModel.DataAnnotations;
using AppSettingsManagerApi.Facades;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AppSettingsManagerApi.Controllers;

// Controllers should be fairly bare-bones.
// Most logic should be handled in the background services like _settingsRepository for this controller
// The controller's purpose is just to define the API's access points

[ApiController]
// This route declaration will add /settings to the end of the base api url
// That will differentiate it from other controllers
[Route("settings")]
public class SettingsController : Controller
{
    private readonly SettingsFacade _settingsFacade;

    public SettingsController(SettingsFacade settingsFacade)
    {
        _settingsFacade = settingsFacade;
    }

    // The user is expected to include the parameters for this method in the request url
    // An example would be https://localhost:xxxx/settings/settingId/TEST/version/1
    //
    // The parameter list includes the [FromRoute] annotations so that it knows to pull these params
    // from the request url
    [HttpGet("userId/{userId}/password/{password}/settingGroupId/{settingGroupId}")]
    public async Task<Model.SettingGroup> GetSettingGroup(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password,
        [FromRoute] [Required] string settingGroupId
    )
    {
        var request = new GetSettingGroupRequest
        {
            UserId = userId,
            Password = password,
            SettingGroupId = settingGroupId
        };
        
        var settingGroup = await _settingsFacade.GetSettingGroup(request);
        return settingGroup;
    }

    [HttpGet("userId/{userId}/password/{password}")]
    public async Task<IEnumerable<Model.SettingGroup>> GetAllSettingGroupsForUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password
    )
    {
        var request = new GetSettingGroupRequest
        {
            UserId = userId,
            Password = password
        };
        
        var settingGroups = await _settingsFacade.GetAllSettingGroupsForUser(request);
        return settingGroups;
    }

    // HttpPost for creating new items
    [HttpPost]
    // This method expects a request object provided in the body (see postman for better visual)
    // HttpClient requests would wrap a CreateSettingRequest object into an HttpContent object and send that
    public async Task<Model.SettingGroup> CreateSettingGroup(
        [FromBody] [Required] CreateSettingRequest request
    )
    {
        var settingGroup = await _settingsFacade.CreateSettingGroup(request);
        return settingGroup;
    }

    // Generally we'll use HttpPut for updates
    [HttpPut]
    public async Task<Model.SettingGroup> UpdateSetting(
        [FromBody] [Required] CreateSettingRequest request
    )
    {
        var settingGroup = await _settingsFacade.UpdateSetting(request);
        return settingGroup;
    }

    [HttpPut("target")]
    public async Task<Model.SettingGroup> ChangeTargetSettingVersion(
        [FromBody] [Required] UpdateTargetSettingRequest request
    )
    {
        var settingGroup = await _settingsFacade.ChangeTargetSettingVersion(request);
        return settingGroup;
    }

    // Adding /delete to route to make sure this isn't called accidentally
    [HttpDelete("userId/{userId}/password/{password}/settingGroupId/{settingGroupId}")]
    public async Task<Model.SettingGroup> DeleteSettingGroup(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password,
        [FromRoute] [Required] string settingGroupId
    )
    {
        var request = new DeleteSettingGroupRequest
        {
            UserId = userId,
            Password = password,
            SettingGroupId = settingGroupId
        };
        
        var settingGroup = await _settingsFacade.DeleteSettingGroup(request);
        return settingGroup;
    }

    [HttpPost("permission")]
    public async Task<Model.Permission> CreateNewPermission([FromBody] [Required] CreatePermissionRequest request)
    {
        return await _settingsFacade.RequestNewSettingGroupAccess(request);
    }

    [HttpPut("permission")]
    public async Task<Model.Permission> UpdatePermission([FromBody] [Required] UpdatePermissionRequest request)
    {
        return await _settingsFacade.UpdateSettingGroupAccess(request);
    }

    [HttpPut("permission/response")]
    public async Task<Model.Permission> PermissionUpdateResponse([FromBody] [Required] PermissionRequestResponse request)
    {
        return await _settingsFacade.PermissionRequestResponse(request);
    }
}
