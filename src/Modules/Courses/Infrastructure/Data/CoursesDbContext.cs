using Courses.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Data;

public class CoursesDbContext : DbContext
{
    public CoursesDbContext(DbContextOptions<CoursesDbContext> options) : base(options) { }
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("courses");
        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.Property(x => x.ProcessedAt)
                .IsRequired(false);
        });
        base.OnModelCreating(modelBuilder);
    }

}
