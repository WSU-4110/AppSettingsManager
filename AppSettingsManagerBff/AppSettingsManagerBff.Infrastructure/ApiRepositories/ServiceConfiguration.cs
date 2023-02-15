using AppSettingsManagerBff.Domain.ApiRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApiRepositories(this IServiceCollection services, AppSettingsManagerApiConfig config)
    {
        services.AddHttpClient();
        services.AddSingleton<IUserRepository, HttpUserRepository>();
        services.AddSingleton<ISettingsRepository, HttpSettingsRepository>();
        services.AddSingleton(config);

        return services;
    }
}