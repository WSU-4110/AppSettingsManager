namespace DefaultNamespace;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApiRepositories(this IServiceCollection services)
    {
        services.AddHttpClient()
        services.AddSingleton<IUserRepository, HttpUserRepository>();
    }
}