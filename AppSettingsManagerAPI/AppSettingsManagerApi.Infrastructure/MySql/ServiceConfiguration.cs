using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public static class ServiceConfiguration
{
    public static IServiceCollection AddMySqlSettingsStorage(
        this IServiceCollection services,
        string connectionString,
        IHealthChecksBuilder healthChecksBuilder
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

        healthChecksBuilder.AddDbContextCheck<SettingsContext>();

        return services;
    }
}
