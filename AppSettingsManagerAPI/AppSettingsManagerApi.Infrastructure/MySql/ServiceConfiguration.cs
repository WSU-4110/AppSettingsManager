using AppSettingsManagerApi.Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public static class ServiceConfiguration
{
    public static IServiceCollection AddMySqlSettingsStorage(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContextPool<SettingsContext>(
            options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                )
        );

        services.AddSingleton<IBaseUserRepository, MySqlBaseUserRepository>();

        return services;
    }
}
