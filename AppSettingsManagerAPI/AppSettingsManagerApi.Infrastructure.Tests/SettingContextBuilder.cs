using AppSettingsManagerApi.Infrastructure.MySql;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.Tests;

public static class SettingContextBuilder
{
    public static SettingsContext BuildTestSettingsContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<SettingsContext>().Options;
        
        var context = new TestSettingsContext(options);

        var createDb = context.Database.GenerateCreateScript();

        createDb = createDb.Replace("\"LastUpdatedAt\" BLOB NOT NULL",
            "\"LastUpdatedAt\" BLOB NOT NULL DEFAULT (randomblob(8))");
        
        context.Database.ExecuteSqlRaw(createDb);

        context.SaveChanges();
        
        return context;
    }
}