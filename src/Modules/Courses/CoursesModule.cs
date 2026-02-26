using Courses.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Courses;

public static class CoursesModule
{
    public static IServiceCollection AddCoursesModule(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("CoursesDb");
        services.AddDbContext<CoursesDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsHistoryTable("__EFMigrationsHistory", "courses");
            }).AddInterceptors(interceptors);
        });
        services.AddScoped<IOutboxProcessor, CoursesOutboxProcessor>();
        // services.AddScoped<VideoProcessedHandler>();
        // services.AddScoped<UploadVideoHandler>();
        return services;

    }
}
