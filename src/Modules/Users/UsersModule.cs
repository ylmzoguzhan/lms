using Courses.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Messaging.Internal;
using Users.Features.CourseReadModels;
using Users.Infrastructure.Data;

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
        return services;

    }
}
