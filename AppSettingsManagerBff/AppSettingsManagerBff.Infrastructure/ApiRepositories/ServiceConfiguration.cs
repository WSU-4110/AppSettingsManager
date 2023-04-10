using AppSettingsManagerBff.Domain.ApiRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public static class ServiceConfiguration
{
    // This gets called in the program.cs file so it runs when the app is built/run
    public static IServiceCollection AddApiRepositories(
        this IServiceCollection services,
        AppSettingsManagerApiConfig config
    )
    {
        // This makes it so we can inject HttpClient into the repositories
        services.AddHttpClient();

        // Registers service for repositories,
        // For instance, when you instantiate IUserRepository, it knows to grab HttpUserRepository
        services.AddSingleton<IUserRepository, HttpUserRepository>();

        // See previous comments
        services.AddSingleton<ISettingsRepository, HttpSettingsRepository>();

        // Makes config available to the repositories, where it shows up as a constructor parameter
        services.AddSingleton(config);

        return services;
    }
}