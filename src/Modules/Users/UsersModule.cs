using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Features.CourseReadModels;
using Users.Features.Enrollments;

namespace Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDb");
        services.AddDbContext<UsersDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsHistoryTable("__EFMigrationsHistory", "users");
            });
        });
        // services.AddScoped<VideoProcessedHandler>();
        // services.AddScoped<UploadVideoHandler>();
        services.AddScoped<IInternalEventHandler<CourseCreatedEvent>, CourseCreatedHandler>();
        services.AddScoped<ICommandHandler<EnrollInCourseCommand, Guid>, EnrollInCourseHandler>();

        return services;

    }
}
