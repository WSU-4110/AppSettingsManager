using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

/// <summary>
/// This will allow us to communicate with the database
/// </summary>
public class SettingsContext : DbContext
{
    public SettingsContext(DbContextOptions<SettingsContext> options)
        : base(options) { }

    // This tells EF to create a table of BaseUsers 
    public DbSet<BaseUser> BaseUsers => Set<BaseUser>();
    // Create a table of Settings
    public DbSet<Setting> Setting => Set<Setting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Declare keys and relationships here
        modelBuilder.Entity<BaseUser>().HasKey(u => u.UserId);
        modelBuilder.Entity<BaseUser>().HasMany(u => u.Settings);

        modelBuilder.Entity<Setting>().HasKey(s => s.Id);
    }
}
