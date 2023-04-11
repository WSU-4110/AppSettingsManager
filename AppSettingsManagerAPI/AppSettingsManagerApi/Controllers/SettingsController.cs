using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
    private readonly SettingFacade _settingFacade;

    public SettingsController(SettingFacade settingFacade)
    {
        _settingFacade = settingFacade;
    }

    #region Get

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

        var settingGroup = await _settingFacade.GetSettingGroup(request);
        return settingGroup;
    }

    [HttpGet("userId/{userId}/password/{password}")]
    public async Task<IEnumerable<Model.SettingGroup>> GetAllSettingGroupsForUser(
        [FromRoute] [Required] string userId,
        [FromRoute] [Required] string password
    )
    {
        var request = new GetSettingGroupRequest { UserId = userId, Password = password };

        var settingGroups = await _settingFacade.GetAllSettingGroupsForUser(request);
        return settingGroups;
    }

    [HttpGet("settingGroupId/{settingGroupId}/version/{version}")]
    public async Task<Dictionary<string, string>> GetSettings(
        [FromRoute] [Required] string settingGroupId,
        [FromRoute] [Required] int version
    )
    {
        var setting = await _settingFacade.GetSettings(settingGroupId, version);
        return setting;
    }
    
    [HttpGet("settingGroupId/{settingGroupId}/version/{version}/variableName/{variableName}")]
    public async Task<string> GetSetting(
        [FromRoute] [Required] string settingGroupId,
        [FromRoute] [Required] int version,
        [FromRoute] [Required] string variableName
    )
    {
        var setting = await _settingFacade.GetSetting(settingGroupId, version, variableName);
        return setting;
    }

    #endregion

    // HttpPost for creating new items
    [HttpPost]
    // This method expects a request object provided in the body (see postman for better visual)
    // HttpClient requests would wrap a CreateSettingRequest object into an HttpContent object and send that
    public async Task<Model.SettingGroup> CreateSettingGroup(
        [FromBody] [Required] CreateSettingRequest request
    )
    {
        var settingGroup = await _settingFacade.CreateSettingGroup(request);
        return settingGroup;
    }

    // Generally we'll use HttpPut for updates
    [HttpPut]
    public async Task<Model.SettingGroup> UpdateSetting(
        [FromBody] [Required] CreateSettingRequest request
    )
    {
        var settingGroup = await _settingFacade.UpdateSetting(request);
        return settingGroup;
    }

    [HttpPut("target")]
    public async Task<Model.SettingGroup> ChangeTargetSettingVersion(
        [FromBody] [Required] UpdateTargetSettingRequest request
    )
    {
        var settingGroup = await _settingFacade.ChangeTargetSettingVersion(request);
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

        var settingGroup = await _settingFacade.DeleteSettingGroup(request);
        return settingGroup;
    }

    [HttpPost("permission")]
    public async Task<Model.Permission> CreateNewPermission(
        [FromBody] [Required] CreatePermissionRequest request
    )
    {
        return await _settingFacade.RequestNewSettingGroupAccess(request);
    }

    [HttpPut("permission")]
    public async Task<Model.Permission> UpdatePermission(
        [FromBody] [Required] UpdatePermissionRequest request
    )
    {
        return await _settingFacade.UpdateSettingGroupAccess(request);
    }

    [HttpPut("permission/response")]
    public async Task<Model.Permission> PermissionUpdateResponse(
        [FromBody] [Required] PermissionRequestResponse request
    )
    {
        return await _settingFacade.PermissionRequestResponse(request);
    }
}
