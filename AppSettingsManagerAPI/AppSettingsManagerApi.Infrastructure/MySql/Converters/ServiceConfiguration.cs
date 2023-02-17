using AppSettingsManagerApi.Domain.Conversion;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerApi.Infrastructure.MySql.Converters;

public static class ServiceConfiguration
{
    public static IServiceCollection AddConverters(this IServiceCollection services)
    {
        services.AddSingleton<
            IBidirectionalConverter<Model.BaseUser, BaseUser>, 
            BaseUserBiverter
        >();
        
        services.AddSingleton<
            IBidirectionalConverter<Model.Setting, Setting>, 
            SettingBiverter
        >();

        return services;
    }
}
