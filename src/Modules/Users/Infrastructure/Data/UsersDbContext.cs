using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure.Data;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<CourseReadModel> CourseReadModels => Set<CourseReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("users");

        base.OnModelCreating(modelBuilder);
    }
}
