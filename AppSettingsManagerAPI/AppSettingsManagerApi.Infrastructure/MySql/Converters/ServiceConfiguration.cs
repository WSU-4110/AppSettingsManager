using AppSettingsManagerApi.Domain.Conversion;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public static class ServiceConfiguration
{
    public static IServiceCollection AddConverters(this IServiceCollection services)
    {
        services.AddSingleton<IBidirectionalConverter<Model.User, User>, UserConverter>();

        services.AddSingleton<IBidirectionalConverter<Model.Setting, Setting>, SettingConverter>();

        services.AddSingleton<
            IBidirectionalConverter<Model.SettingGroup, SettingGroup>,
            SettingGroupConverter
        >();

        services.AddSingleton<
            IBidirectionalConverter<Model.Permission, Permission>,
            PermissionConverter
        >();

        return services;
    }
}
