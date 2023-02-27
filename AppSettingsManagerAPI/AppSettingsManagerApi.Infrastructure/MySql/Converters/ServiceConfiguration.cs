using AppSettingsManagerApi.Domain.Conversion;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public static class ServiceConfiguration
{
    public static IServiceCollection AddConverters(this IServiceCollection services)
    {
        services.AddSingleton<
            IBidirectionalConverter<Model.User, User>,
            UserBiverter
        >();

        services.AddSingleton<
            IBidirectionalConverter<Model.SettingVersion, SettingVersion>,
            SettingVersionBiverter
        >();

        services.AddSingleton<
            IBidirectionalConverter<Model.SettingGroup, SettingGroup>,
            SettingGroupBiverter
        >();

        services.AddSingleton<
            IBidirectionalConverter<Model.Permission, Permission>,
            PermissionBiverter
        >();

        return services;
    }
}
