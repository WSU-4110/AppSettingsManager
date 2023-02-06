using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;

public class SettingsContext : DbContext
{
    public SettingsContext(DbContextOptions<SettingsContext> options): base(options) { }

    public DbSet<BaseUser> BaseUsers => Set<BaseUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseUser>().HasKey(u => u.UserId);
    }
}