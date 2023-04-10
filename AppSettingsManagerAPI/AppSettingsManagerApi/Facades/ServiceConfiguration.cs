namespace AppSettingsManagerApi.Facades;

public static class ServiceConfiguration
{
    public static IServiceCollection AddFacades(this IServiceCollection services)
    {
        services.AddScoped<SettingsFacade>();
        services.AddScoped<UserFacade>();

        return services;
    }
}