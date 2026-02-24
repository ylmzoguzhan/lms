using Courses.Infrastructure.Data;
using Courses.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Messaging.Outbox;

namespace Courses;

public static class CoursesModule
{
    public static IServiceCollection AddCoursesModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CoursesDb");
        services.AddDbContext<CoursesDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsHistoryTable("__EFMigrationsHistory", "courses");
            });
        });
        services.AddScoped<IOutboxProcessor, CoursesOutboxProcessor>();
        // services.AddScoped<VideoProcessedHandler>();
        // services.AddScoped<UploadVideoHandler>();
        return services;

    }
}
