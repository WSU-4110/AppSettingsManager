using AppSettingsManagerApi.Infrastructure.MySql;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.Tests;

public class TestSettingsContext : SettingsContext
{
    public TestSettingsContext(DbContextOptions<SettingsContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        optionsBuilder.UseSqlite(connection);
    }
}
