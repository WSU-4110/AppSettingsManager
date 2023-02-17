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
        var setting = await _settingsRepository.CreateSetting(request);
        return setting;
    }
}
