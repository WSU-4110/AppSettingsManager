using System.ComponentModel.DataAnnotations;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model.Api;
using Microsoft.AspNetCore.Mvc;

namespace AppSettingsManagerBff.Controllers;

[ApiController]
[Route("settings")]
public class SettingsController : Controller
{
    private readonly ISettingsRepository _settingsRepository;

    public SettingsController(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    [HttpPost]
    public async Task<ApiSetting> CreateSetting(CreateSettingRequest request)
    {
        return await _settingsRepository.CreateSetting(request);
    }

    [HttpGet("settingId/{settingId}/version/{version}")]
    public async Task<ApiSetting> GetSetting(
        [FromRoute] [Required] string settingId,
        [FromRoute] [Required] int version
    )
    {
        return await _settingsRepository.GetSetting(settingId, version);
    }

    [HttpPut]
    public async Task<ApiSetting> UpdateSetting([FromRoute] [Required] UpdateSettingRequest request)
    {
        return await _settingsRepository.UpdateSetting(request);
    }

    [HttpDelete("delete/settingId/{settingId}")]
    public async Task<IEnumerable<ApiSetting>> DeleteSetting(
        [FromRoute] [Required] string settingId
    )
    {
        return await _settingsRepository.DeleteSetting(settingId);
    }
}
