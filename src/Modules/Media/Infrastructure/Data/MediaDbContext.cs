using MassTransit;
using Media.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Media.Infrastructure.Data;

public class MediaDbContext : DbContext
{
    public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options) { }

    public DbSet<Video> Videos => Set<Video>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("media");

        base.OnModelCreating(modelBuilder);
    }
}
