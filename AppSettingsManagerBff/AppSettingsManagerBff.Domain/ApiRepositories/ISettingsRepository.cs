using AppSettingsManagerBff.Model.Api;

namespace AppSettingsManagerBff.Domain.ApiRepositories;

public interface ISettingsRepository
{
    Task<ApiSetting> CreateSetting(CreateSettingRequest request);
}
