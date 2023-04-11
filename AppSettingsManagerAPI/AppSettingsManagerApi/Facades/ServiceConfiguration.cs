namespace AppSettingsManagerApi.Facades;

public static class ServiceConfiguration
{
    public static IServiceCollection AddFacades(this IServiceCollection services)
    {
        services.AddScoped<SettingFacade>();

        return services;
    }
}
