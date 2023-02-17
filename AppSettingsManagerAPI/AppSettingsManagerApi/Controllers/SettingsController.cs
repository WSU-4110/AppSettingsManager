using System.ComponentModel.DataAnnotations;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model;
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
    private readonly ISettingsRepository _settingsRepository;

    public SettingsController(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    // The user is expected to include the parameters for this method in the request url
    // An example would be https://localhost:xxxx/settings/settingId/TEST/version/1
    //
    // The parameter list includes the [FromRoute] annotations so that it knows to pull these params
    // from the request url
    [HttpGet("settingId/{settingId}/version/{version}")]
    public async Task<Model.Setting> GetSetting([FromRoute] [Required] string settingId,
        [FromRoute] [Required] int version)
    {
        return await _settingsRepository.GetSetting(settingId, version);
    }

    [HttpGet("settingId/{settingId}")]
    public async Task<IEnumerable<Model.Setting>> GetAllSettingVersions([FromRoute] [Required] string settingId)
    {
        return await _settingsRepository.GetAllSettingVersions(settingId);
    }
    
    // HttpPost for creating new items
    [HttpPost]
    // This method expects a request object provided in the body (see postman for better visual)
    // HttpClient requests would wrap a CreateSettingRequest object into an HttpContent object and send that
    public async Task<Model.Setting> CreateSetting([FromBody] [Required] CreateSettingRequest request)
    {
        return await _settingsRepository.CreateSetting(request);
    }
    
    // Generally we'll use HttpPut for updates
    [HttpPut]
    public async Task<Model.Setting> UpdateSetting([FromBody] [Required] UpdateSettingRequest request)
    {
        return await _settingsRepository.UpdateSetting(request);
    }
    
    // Adding /delete to route to make sure this isn't called accidentally
    [HttpDelete("delete/settingId/{settingId}")]
    public async Task<IEnumerable<Model.Setting>> DeleteSettings([FromRoute] [Required] string settingId)
    {
        return await _settingsRepository.DeleteSetting(settingId);
    }
}
