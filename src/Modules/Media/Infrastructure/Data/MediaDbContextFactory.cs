using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Media.Infrastructure.Data;


public class MediaDbContextFactory() : IDesignTimeDbContextFactory<MediaDbContext>
{
    public MediaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MediaDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=lms_db;Username=admin;Password=admin1234;SearchPath=media",
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "media"));

        return new MediaDbContext(optionsBuilder.Options);
    }
}