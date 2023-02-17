using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AppSettingsManagerApi.Infrastructure.MySql;

/// <summary>
/// This is just something internal that allows us to create migrations, shouldn't need to change
/// </summary>
internal class SettingsContextFactory : IDesignTimeDbContextFactory<SettingsContext>
{
    public SettingsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SettingsContext>();
        var connectionString = "Server=localhost;Database=db;Uid=user;Pwd=password;";

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new SettingsContext(optionsBuilder.Options);
    }
}
