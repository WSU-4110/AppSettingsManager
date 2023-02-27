using AppSettingsManagerApi.Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsManagerApi.Infrastructure.MySql;

/// <summary>
/// These static classes are a clean way of registering all of your interfaces/repositories
/// Breaking things up into chunks like this is preferable to throwing all of this into the program.cs file
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// Adds connection to database and scoped service for our MySql repositories
    /// Repositories must be scoped because the context objects that they use to communicate with database are scoped
    /// </summary>
    /// <param name="services">
    /// This is the same IServiceCollection that is being used in
    /// the program.cs file before calling builder.build()
    ///
    /// Using the "this" keyword in this parameter list means this method can be called
    /// simply like 'services.AddMySqlStorage(connectionString)'
    /// </param>
    /// <param name="connectionString">
    /// This is how the database location and login info is passed in so that we can connect to the MySql db
    /// </param>
    /// <returns>
    /// The same IServiceCollection object that was initially referenced
    /// </returns>
    public static IServiceCollection AddMySqlSettingsStorage(
        this IServiceCollection services,
        string connectionString
    )
    {
        // This actually configures the MySql db connection
        services.AddDbContextPool<SettingsContext>(
            options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                )
        );

        services.AddScoped<IUserRepository, MySqlUserRepository>();
        services.AddScoped<ISettingRepository, MySqlSettingRepository>();

        return services;
    }
}
