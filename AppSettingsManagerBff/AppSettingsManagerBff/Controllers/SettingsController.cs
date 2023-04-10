using System.ComponentModel.DataAnnotations;
using AppSettingsManagerBff.Domain.ApiRepositories;
using AppSettingsManagerBff.Model;
using AppSettingsManagerBff.Model.Requests;
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

    [HttpGet("userId/{userId}/password/{password}/settingGroupId/{settingGroupId}")]
    public async Task<SettingGroup> GetSettingGroup(
        [FromRoute][Required] string userId,
        [FromRoute][Required] string password,
        [FromRoute][Required] string settingGroupId
    )
    {
        var settingGroup = await _settingsRepository.GetSettingGroup(
            userId,
            password,
            settingGroupId
        );
        return settingGroup;
    }

    [HttpGet("userId/{userId}/password/{password}")]
    public async Task<IEnumerable<SettingGroup>> GetAllSettingGroupsForUser(
        [FromRoute][Required] string userId,
        [FromRoute][Required] string password
    )
    {
        var settingGroup = await _settingsRepository.GetSettingGroupsForUser(userId, password);
        return settingGroup;
    }

    [HttpPost]
    public async Task<SettingGroup> CreateSettingGroup(
        [FromBody][Required] CreateSettingRequest request
    )
    {
        var settingGroup = await _settingsRepository.CreateSettingGroup(request);
        return settingGroup;
    }

    [HttpPut]
    public async Task<SettingGroup> UpdateSetting(CreateSettingRequest request)
    {
        var settingGroup = await _settingsRepository.UpdateSetting(request);
        return settingGroup;
    }

    [HttpPut("target")]
    public async Task<SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request)
    {
        var settingGroup = await _settingsRepository.ChangeTargetSettingVersion(request);
        return settingGroup;
    }

    [HttpDelete("userId/{userId}/password/{password}/settingGroupId/{settingGroupId}")]
    public async Task<SettingGroup> DeleteSettingGroup(
        [FromRoute][Required] string userId,
        [FromRoute][Required] string password,
        [FromRoute][Required] string settingGroupId
    )
    {
        var settingGroup = await _settingsRepository.DeleteSettingGroup(
            userId,
            password,
            settingGroupId
        );
        return settingGroup;
    }
}