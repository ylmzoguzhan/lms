using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Users.Infrastructure.Data;

public class UsersDbContextFactory() : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=lms_db;Username=admin;Password=admin1234;SearchPath=users",
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "users"));

        return new UsersDbContext(optionsBuilder.Options);
    }
}
