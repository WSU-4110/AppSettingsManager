using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

/// <summary>
/// This will allow us to communicate with the database
/// </summary>
public class SettingsContext : DbContext
{
    public SettingsContext(DbContextOptions<SettingsContext> options)
        : base(options) { }

    // This tells EF to create a table of Users
    public DbSet<User> Users => Set<User>();

    // Create overarching SettingGroup table
    public DbSet<SettingGroup> SettingGroups => Set<SettingGroup>();

    // Create a table of Settings Versions
    public DbSet<Setting> Settings => Set<Setting>();

    // Permissions table
    public DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Declare keys and relationships here
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<User>().HasMany(u => u.Settings);

        modelBuilder.Entity<SettingGroup>().HasKey(s => s.SettingId);

        modelBuilder.Entity<Setting>().HasKey(s => new { Id = s.SettingGroupId, s.Version });
        modelBuilder.Entity<Setting>().HasIndex(s => s.SettingGroupId);
        modelBuilder.Entity<Setting>().HasIndex(s => s.IsCurrent);
        modelBuilder
            .Entity<Setting>()
            .HasOne<SettingGroup>(sv => sv.SettingGroup)
            .WithMany(sg => sg.Settings);

        modelBuilder.Entity<Permission>().HasKey(p => new { p.UserId, p.SettingGroupId });
        modelBuilder.Entity<Permission>().HasOne<User>(p => p.User);
        modelBuilder.Entity<Permission>().HasOne<SettingGroup>(p => p.SettingGroup);
    }
}
